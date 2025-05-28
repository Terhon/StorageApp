using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Controllers;
using StorageWeb.Data;
using StorageWeb.Models;

namespace StorageApp.UnitTests;

public class RecipeControllerTests
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
        context.IngredientItems.Add(new IngredientItem{ ItemId = 1, Amount = 30, RecipeId = recipe2.Id});
        
        context.SaveChanges();
    }
    
    [Fact]
    public async Task Index_ReturnsViewResult_WhenRecipesExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);
        
        var result = await controller.Index();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Recipe>>(viewResult.ViewData.Model);
        model.Count().Should().Be(2);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WhenRecipesExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);
        
        var recipe = context.Recipe.First();
        
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
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);
        
        var result = controller.Create();
        
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async Task Create_AddsRecipe_WhenValid()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);

        var recipeCount = context.Recipe.Count();
        
        var newRecipe = new Recipe { Name = "New Recipe", Description = "New Recipe Description" };
        var result = await controller.Create(newRecipe);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.Recipe.Count().Should().Be(recipeCount + 1);
        context.Recipe.Should().Contain(newRecipe);
    }
    
    [Fact]
    public async Task Create_ReturnsViewResult_WhenInvalid()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);
        
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
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);
        
        var recipe = context.Recipe.First();
        
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
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);
        
        var result = await controller.Edit(null);
        
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Edit_ModifiesItem_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);
        
        var newName = "New Name";
        var newDescription = "New Description";
        
        var editedRecipe = context.Recipe.First();
        editedRecipe.Description = newDescription;
        editedRecipe.Name = newName;
        var result = await controller.Edit(editedRecipe.Id, editedRecipe);
        
        Assert.IsType<RedirectToActionResult>(result);
        var recipe = context.Recipe.SingleOrDefault(x => x.Id == editedRecipe.Id);
        recipe.Should().NotBeNull();
        recipe.Name.Should().Be(newName);
        recipe.Description.Should().Be(newDescription);
        recipe.Ingredients.Should().BeEquivalentTo(editedRecipe.Ingredients);
    }
    
    [Fact]
    public async Task Delete_ReturnsViewResult_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);
        
        var recipe = context.Recipe.First();
        
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
        var context = GetInMemoryStorageWebContext();
        var controller = new RecipeController(context);

        var recipeCount = context.Recipe.Count();
        var recipe = context.Recipe.First();
        
        var result = await controller.DeleteConfirmed(recipe.Id);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.Recipe.Count().Should().Be(recipeCount - 1);
    }
}