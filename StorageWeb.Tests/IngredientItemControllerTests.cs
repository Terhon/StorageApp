using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Controllers;
using StorageWeb.Data;
using StorageWeb.Models;

namespace StorageApp.UnitTests;

public class IngredientItemControllerTests
{
     private StorageWebContext GetInMemoryStorageWebContext()
    {
        var options = new DbContextOptionsBuilder<StorageWebContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var context = new StorageWebContext(options);
        
        Seed(context);
        
        return context;
    }

    private void Seed(StorageWebContext context)
    {
        context.Item.Add(new Item{ Name = "Item 1" , Unit = "unit 1"});
        context.Item.Add(new Item{ Name = "Item 2" , Unit = "unit 2"});

        var recipe1 = new Recipe{ Name = "Recipe 1", Description = "Recipe Description 1"};
        context.Recipe.Add(recipe1);
        var recipe2 = new Recipe{ Name = "Recipe 2", Description = "Recipe Description 2"};
        context.Recipe.Add(recipe2);

        context.IngredientItems.Add(new IngredientItem{ ItemId = 1, Amount = 2, RecipeId = recipe1.Id});
        context.IngredientItems.Add(new IngredientItem{ ItemId = 2, Amount = 5, RecipeId = recipe1.Id});
        context.IngredientItems.Add(new IngredientItem{ ItemId = 1, Amount = 30, RecipeId = recipe1.Id});
        
        context.SaveChanges();
    }
    
    [Fact]
    public async Task Index_ReturnsViewResult_WhenIngredientItemsExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);

        var ingredientItemsCount = context.IngredientItems.Count();
        
        var result = await controller.Index();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<IngredientItem>>(viewResult.ViewData.Model);
        model.Count().Should().Be(ingredientItemsCount);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WhenRecipesExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);
        
        var ingredientItem = context.IngredientItems
            .Include(ingredientItem => ingredientItem.Recipe)
            .Include(ingredientItem => ingredientItem.Item).First();
        
        var result = await controller.Details(ingredientItem.Id);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IngredientItem>(viewResult.ViewData.Model);
        model.Amount.Should().Be(ingredientItem.Amount);
        model.Recipe.Should().Be(ingredientItem.Recipe);
        model.RecipeId.Should().Be(ingredientItem.RecipeId);
        model.Item.Should().Be(ingredientItem.Item);
        model.ItemId.Should().Be(ingredientItem.ItemId);
    }
    
    [Fact]
    public async Task Details_ReturnsNotFoundResult_WhenWrongId()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);
        
        var result = await controller.Details(-1);
        
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_ReturnsViewResult()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);
        
        var result = controller.Create();
        
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async Task Create_AddsIngredientItem_WhenValid()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);

        var ingredientItemsCount = context.IngredientItems.Count();
        
        var newItem = new IngredientItem { Amount = 5, Recipe = context.Recipe.First(), Item = context.Item.First() };
        var result = await controller.Create(newItem);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.IngredientItems.Count().Should().Be(ingredientItemsCount + 1);
        context.IngredientItems.Should().Contain(newItem);
    }
    
    [Fact]
    public async Task Edit_ReturnsViewResult_WhenRecipesExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);
        
        var ingredientItem = context.IngredientItems
            .Include(ingredientItem => ingredientItem.Recipe)
            .Include(ingredientItem => ingredientItem.Item).First();
        
        var result = await controller.Edit(ingredientItem.Id);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IngredientItem>(viewResult.ViewData.Model);
        model.Amount.Should().Be(ingredientItem.Amount);
        model.Recipe.Should().Be(ingredientItem.Recipe);
        model.RecipeId.Should().Be(ingredientItem.RecipeId);
        model.Item.Should().Be(ingredientItem.Item);
        model.ItemId.Should().Be(ingredientItem.ItemId);
    }

    [Fact]
    public async Task Edit_ReturnsNotFoundResult_WhenNullId()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);
        
        var result = await controller.Edit(null);
        
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Edit_ModifiesItem_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);
        
        var editedIngredientItem = context.IngredientItems.First();
        var newAmount = editedIngredientItem.Amount + new Random().Next();
        editedIngredientItem.Amount = newAmount;
        var result = await controller.Edit(editedIngredientItem.Id, editedIngredientItem);
        
        Assert.IsType<RedirectToActionResult>(result);
        var ingredientItem = context.IngredientItems.SingleOrDefault(x => x.Id == editedIngredientItem.Id);
        ingredientItem.Should().NotBeNull();
        ingredientItem.Amount.Should().Be(newAmount);
    }
    
    [Fact]
    public async Task Delete_ReturnsViewResult_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);
        
        var ingredientItem = context.IngredientItems
            .Include(ingredientItem => ingredientItem.Recipe)
            .Include(ingredientItem => ingredientItem.Item).First();
        
        var result = await controller.Delete(ingredientItem.Id);
        var viewResult = Assert.IsType<ViewResult>(result);
            
        var model = Assert.IsAssignableFrom<IngredientItem>(viewResult.ViewData.Model);
        model.Amount.Should().Be(ingredientItem.Amount);
        model.Recipe.Should().Be(ingredientItem.Recipe);
        model.RecipeId.Should().Be(ingredientItem.RecipeId);
        model.Item.Should().Be(ingredientItem.Item);
        model.ItemId.Should().Be(ingredientItem.ItemId);
    }
    
    [Fact]
    public async Task DeleteConfirmed_RemovesItem_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new IngredientItemController(context);

        var ingredientItemCount = context.IngredientItems.Count();
        var recipe = context.IngredientItems.First();
        
        var result = await controller.DeleteConfirmed(recipe.Id);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.IngredientItems.Count().Should().Be(ingredientItemCount - 1);
    }
}