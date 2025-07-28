using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Storage.Application.IngredientItem.Commands;
using Storage.Application.IngredientItem.Queries;
using Storage.Application.ItemType.Queries;
using Storage.Application.Recipe.Queries;
using Storage.Domain.Entities;
using Storage.Infrastructure.Data;
using Storage.Infrastructure.Services.IngredientItem;
using Storage.Infrastructure.Services.ItemType;
using Storage.Infrastructure.Services.Recipe;
using StorageWeb.Controllers;
using StorageWeb.Models;
using IngredientItem = StorageWeb.Models.IngredientItem;
using Recipe = Storage.Domain.Entities.Recipe;

namespace StorageApp.UnitTests;

public class IngredientItemControllerTests
{
    private (IIngredientItemCommandService, IIngredientItemQueryService,
        IItemTypeQueryService, IRecipeQueryService, StorageDbContext) GetInMemoryStorageWebContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<StorageDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new StorageDbContext(options);
        context.Database.EnsureCreated();

        Seed(context);

        return (new IngredientItemCommandService(context), new IngredientItemQueryService(context),
            new ItemTypeQueryService(context), new RecipeQueryService(context), context);
    }

    private void Seed(StorageDbContext context)
    {
        var itemTypes = new List<ItemType>
        {
            new() { Id = 1, Name = "Item 1", Unit = "unit 1" },
            new() { Id = 2, Name = "Item 2", Unit = "unit 2" }
        };
        context.ItemTypes.Add(itemTypes[0]);
        context.ItemTypes.Add(itemTypes[1]);

        var recipe1 = new Recipe { Name = "Recipe 1", Description = "Recipe Description 1" };
        context.Recipes.Add(recipe1);
        var recipe2 = new Recipe { Name = "Recipe 2", Description = "Recipe Description 2" };
        context.Recipes.Add(recipe2);

        context.IngredientItems.Add(new Storage.Domain.Entities.IngredientItem
            { ItemType = itemTypes[0], Amount = 2, Recipe = recipe1 });
        context.IngredientItems.Add(new Storage.Domain.Entities.IngredientItem
            { ItemType = itemTypes[1], Amount = 5, Recipe = recipe1 });
        context.IngredientItems.Add(new Storage.Domain.Entities.IngredientItem
            { ItemType = itemTypes[0], Amount = 30, Recipe = recipe1 });

        context.SaveChanges();
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WhenIngredientItemsExists()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var ingredientItemsCount = context.IngredientItems.Count();

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<IngredientItem>>(viewResult.ViewData.Model);
        model.Count().Should().Be(ingredientItemsCount);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WhenRecipesExists()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var ingredientItem = context.IngredientItems
            .Include(ingredientItem => ingredientItem.Recipe)
            .Include(ingredientItem => ingredientItem.ItemType).First();

        var result = await controller.Details(ingredientItem.Id);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IngredientItem>(viewResult.ViewData.Model);
        model.Amount.Should().Be(ingredientItem.Amount);
        //model.Recipe.Should().Be(ingredientItem.Recipe);
        model.RecipeId.Should().Be(ingredientItem.Recipe.Id);
        //model.Item.Should().Be(ingredientItem.Item);
        model.ItemId.Should().Be(ingredientItem.ItemType.Id);
    }

    [Fact]
    public async Task Details_ReturnsNotFoundResult_WhenWrongId()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var result = await controller.Details(-1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsViewResult()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var result = await controller.Create();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_AddsIngredientItem_WhenValid()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var ingredientItemsCount = context.IngredientItems.Count();

        var newItem = new IngredientItem
        {
            Amount = 5,
            Recipe = new StorageWeb.Models.Recipe { Id = context.Recipes.First().Id },
            Item = new Item { Id = context.ItemTypes.First().Id }
        };
        newItem.RecipeId = newItem.Recipe.Id;
        newItem.ItemId = newItem.Item.Id;
        
        var result = await controller.Create(newItem);

        Assert.IsType<RedirectToActionResult>(result);
        context.IngredientItems.Count().Should().Be(ingredientItemsCount + 1);
        //context.IngredientItems.Should().Contain(newItem);
        context.IngredientItems.Should().ContainSingle(i => i.Recipe.Id == newItem.Recipe.Id
                                                             && i.ItemType.Id == newItem.Item.Id
                                                            && i.Amount == newItem.Amount);
    }

    [Fact]
    public async Task Edit_ReturnsViewResult_WhenRecipesExists()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var ingredientItem = context.IngredientItems
            .Include(ingredientItem => ingredientItem.Recipe)
            .Include(ingredientItem => ingredientItem.ItemType).First();

        var result = await controller.Edit(ingredientItem.Id);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IngredientItem>(viewResult.ViewData.Model);
        model.Amount.Should().Be(ingredientItem.Amount);
        //model.Recipe.Should().Be(ingredientItem.Recipe);
        model.RecipeId.Should().Be(ingredientItem.Recipe.Id);
        //model.Item.Should().Be(ingredientItem.ItemType);
        model.ItemId.Should().Be(ingredientItem.ItemType.Id);
    }

    [Fact]
    public async Task Edit_ReturnsNotFoundResult_WhenNullId()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var result = await controller.Edit(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ModifiesItem_WhenItemExists()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var editedIngredientItem = context.IngredientItems.First();
        var newAmount = editedIngredientItem.Amount + new Random().Next();
        editedIngredientItem.Amount = newAmount;
        var result = await controller.Edit(editedIngredientItem.Id, new IngredientItem
        {
            Id = editedIngredientItem.Id,
            Amount = editedIngredientItem.Amount,
            RecipeId = editedIngredientItem.Recipe.Id,
            ItemId = editedIngredientItem.ItemType.Id
        });

        Assert.IsType<RedirectToActionResult>(result);
        var ingredientItem = context.IngredientItems.SingleOrDefault(x => x.Id == editedIngredientItem.Id);
        ingredientItem.Should().NotBeNull();
        ingredientItem.Amount.Should().Be(newAmount);
    }

    [Fact]
    public async Task Delete_ReturnsViewResult_WhenItemExists()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var ingredientItem = context.IngredientItems
            .Include(ingredientItem => ingredientItem.Recipe)
            .Include(ingredientItem => ingredientItem.ItemType).First();

        var result = await controller.Delete(ingredientItem.Id);
        var viewResult = Assert.IsType<ViewResult>(result);

        var model = Assert.IsAssignableFrom<IngredientItem>(viewResult.ViewData.Model);
        model.Amount.Should().Be(ingredientItem.Amount);
        //model.Recipe.Should().Be(ingredientItem.Recipe);
        model.RecipeId.Should().Be(ingredientItem.Recipe.Id);
        //model.Item.Should().Be(ingredientItem.ItemType);
        model.ItemId.Should().Be(ingredientItem.ItemType.Id);
    }

    [Fact]
    public async Task DeleteConfirmed_RemovesItem_WhenItemExists()
    {
        var (commandService, queryService, itemTypeQueryService, recipeQueryService, context) =
            GetInMemoryStorageWebContext();
        var controller =
            new IngredientItemController(commandService, queryService, itemTypeQueryService, recipeQueryService);

        var ingredientItemCount = context.IngredientItems.Count();
        var recipe = context.IngredientItems.First();

        var result = await controller.DeleteConfirmed(recipe.Id);

        Assert.IsType<RedirectToActionResult>(result);
        context.IngredientItems.Count().Should().Be(ingredientItemCount - 1);
    }
}