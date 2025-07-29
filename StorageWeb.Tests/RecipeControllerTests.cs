using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Storage.Application.IngredientItem.Commands;
using Storage.Application.Recipe.Commands;
using Storage.Application.Recipe.Queries;
using Storage.Domain.Entities;
using Storage.Infrastructure.Data;
using Storage.Infrastructure.Services.IngredientItem;
using Storage.Infrastructure.Services.Recipe;
using StorageWeb.Controllers;
using Recipe = StorageWeb.Models.Recipe;

namespace StorageApp.UnitTests;

public class RecipeControllerTests
{
    private (IRecipeCommandService, IRecipeQueryService,
        IIngredientItemCommandService, StorageDbContext) GetInMemoryStorageWebContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<StorageDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new StorageDbContext(options);
        context.Database.EnsureCreated();

        Seed(context);

        return (new RecipeCommandService(context), new RecipeQueryService(context),
            new IngredientItemCommandService(context), context);
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

        var recipe1 = new Storage.Domain.Entities.Recipe { Name = "Recipe 1", Description = "Recipe Description 1" };
        context.Recipes.Add(recipe1);
        var recipe2 = new Storage.Domain.Entities.Recipe { Name = "Recipe 2", Description = "Recipe Description 2" };
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
    public async Task Index_ReturnsViewResult_WhenRecipesExists()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Recipe>>(viewResult.ViewData.Model);
        model.Count().Should().Be(2);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WhenRecipesExists()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var recipe = context.Recipes.First();

        var result = await controller.Details(recipe.Id);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Recipe>(viewResult.ViewData.Model);
        model.Name.Should().Be(recipe.Name);
        model.Description.Should().Be(recipe.Description);
        model.Ingredients.Should().BeEquivalentTo(recipe.Ingredients);
    }

    [Fact]
    public void Create_ReturnsViewResult()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var result = controller.Create();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_AddsRecipe_WhenValid()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var recipeCount = context.Recipes.Count();

        var newRecipe = new Recipe { Name = "New Recipe", Description = "New Recipe Description" };
        var result = await controller.Create(newRecipe);

        Assert.IsType<RedirectToActionResult>(result);
        context.Recipes.Count().Should().Be(recipeCount + 1);
        context.Recipes.Should().ContainSingle(i => i.Name == newRecipe.Name
                                                    && i.Description == newRecipe.Description);
    }

    [Fact]
    public async Task Create_ReturnsViewResult_WhenInvalid()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var newRecipe = new Recipe { Name = new string('x', 61), Description = new string('x', 1001) };

        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(newRecipe, new ValidationContext(newRecipe), validationResults, true);
        foreach (var valResult in validationResults)
        foreach (var memberName in valResult.MemberNames)
            controller.ModelState.AddModelError(memberName, valResult.ErrorMessage ?? "Invalid");

        var result = await controller.Create(newRecipe);

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Edit_ReturnsViewResult_WhenRecipesExists()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var recipe = context.Recipes.First();

        var result = await controller.Edit(recipe.Id);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Recipe>(viewResult.ViewData.Model);
        model.Name.Should().Be(recipe.Name);
        model.Description.Should().Be(recipe.Description);
        model.Ingredients.Should().BeEquivalentTo(recipe.Ingredients);
    }

    [Fact]
    public async Task Edit_ReturnsNotFoundResult_WhenNullId()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var result = await controller.Edit(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ModifiesItem_WhenItemExists()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var newName = "New Name";
        var newDescription = "New Description";

        var editedRecipe = context.Recipes.First();
        editedRecipe.Description = newDescription;
        editedRecipe.Name = newName;
        var result = await controller.Edit(editedRecipe.Id,
            new Recipe
            {
                Id = editedRecipe.Id,
                Name = newName,
                Description = newDescription,
                //Ingredients = editedRecipe.Ingredients
            });

        Assert.IsType<RedirectToActionResult>(result);
        var recipe = context.Recipes.SingleOrDefault(x => x.Id == editedRecipe.Id);
        recipe.Should().NotBeNull();
        recipe.Name.Should().Be(newName);
        recipe.Description.Should().Be(newDescription);
        recipe.Ingredients.Should().BeEquivalentTo(editedRecipe.Ingredients);
    }

    [Fact]
    public async Task Delete_ReturnsViewResult_WhenItemExists()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var recipe = context.Recipes.First();

        var result = await controller.Delete(recipe.Id);
        var viewResult = Assert.IsType<ViewResult>(result);

        var model = Assert.IsAssignableFrom<Recipe>(viewResult.ViewData.Model);
        model.Name.Should().Be(recipe.Name);
        model.Description.Should().Be(recipe.Description);
        model.Ingredients.Should().BeEquivalentTo(recipe.Ingredients);
    }

    [Fact]
    public async Task DeleteConfirmed_RemovesItem_WhenItemExists()
    {
        var (commandService, queryService, ingredientItemCommandService, context) = GetInMemoryStorageWebContext();
        var controller = new RecipeController(commandService, queryService, ingredientItemCommandService);

        var recipeCount = context.Recipes.Count();
        var recipe = context.Recipes.First();

        var result = await controller.DeleteConfirmed(recipe.Id);

        Assert.IsType<RedirectToActionResult>(result);
        context.Recipes.Count().Should().Be(recipeCount - 1);
    }
}