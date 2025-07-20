using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Storage.Application.Interfaces;
using Storage.Domain.Entities;
using Storage.Infrastructure.Data;
using Storage.Infrastructure.Services;
using StorageWeb.Controllers;
using StorageWeb.Models;

namespace StorageApp.UnitTests;

public class ItemControllerTests
{
    private (IItemTypeRepository, StorageDbContext) GetRepositoryAndInMemoryDbContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<StorageDbContext>()
            .UseSqlite(connection)
            .Options;
        
        var context = new StorageDbContext(options);
        context.Database.EnsureCreated();
        Seed(context);
        
        return (new ItemTypeRepository(context), context);
    }

    private static void Seed(StorageDbContext context)
    {
        context.ItemTypes.Add(new ItemType{ Name = "Item 1" , Unit = "unit 1"});
        context.ItemTypes.Add(new ItemType{ Name = "Item 2" , Unit = "unit 2"});
        
        context.SaveChanges();
    }
    
    [Fact]
    public async Task Index_ReturnsViewResult_WhenItemExists()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var result = await controller.Index();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Item>>(viewResult.ViewData.Model);
        model.Count().Should().Be(2);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WhenItemExists()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var result = await controller.Details(1);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Item>(viewResult.ViewData.Model);
        model.Name.Should().Be("Item 1");
        model.Unit.Should().Be("unit 1");
    }

    [Fact]
    public void Create_ReturnsViewResult()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var result = controller.Create();
        
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async Task Create_AddsItemToItemController_WhenValid()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var newItem = new Item { Name = "New Item", Unit = "New Unit" };
        var result = await controller.Create(newItem);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.ItemTypes.Count().Should().Be(3);
        context.ItemTypes.Should().ContainSingle(i => i.Name == newItem.Name && i.Unit == newItem.Unit );
    }
    
    [Fact]
    public async Task Create_AddsItemToItemController_WhenInvalid()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var newItem = new Item{ Name = new string('x', 61), Unit = new string('u', 21) };
        
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(newItem, new ValidationContext(newItem), validationResults, true);
        foreach (var valResult in validationResults)
            foreach (var memberName in valResult.MemberNames)
                controller.ModelState.AddModelError(memberName, valResult.ErrorMessage ?? "Invalid");
        
        var result = await controller.Create(newItem);
        
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async Task Edit_ReturnsViewResult_WhenItemExists()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var result = await controller.Edit(1);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Item>(viewResult.ViewData.Model);
        model.Name.Should().Be("Item 1");
        model.Unit.Should().Be("unit 1");
    }

    [Fact]
    public async Task Edit_ReturnsNotFoundResult_WhenNullId()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var result = await controller.Edit(null);
        
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Edit_ModifiesItem_WhenItemExists()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var newName = "New Name";
        var newUnit = "New Unit";
        
        var editedItem = context.ItemTypes.First();
        var result = await controller.Edit(editedItem.Id, new Item { Id = editedItem.Id, Name = newName, Unit = newUnit });
        
        Assert.IsType<RedirectToActionResult>(result);
        var item = context.ItemTypes.SingleOrDefault(x => x.Id == 1);
        item.Should().NotBeNull();
        item.Name.Should().Be(newName);
        item.Unit.Should().Be(newUnit);
    }
    
    [Fact]
    public async Task Delete_ReturnsViewResult_WhenItemExists()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);
        
        var result = await controller.Delete(1);
        var viewResult = Assert.IsType<ViewResult>(result);
            
        var model = Assert.IsAssignableFrom<Item>(viewResult.ViewData.Model);
        model.Name.Should().Be("Item 1");
        model.Unit.Should().Be("unit 1");
    }
    
    [Fact]
    public async Task DeleteConfirmed_RemovesItem_WhenItemExists()
    {
        var (repository, context) = GetRepositoryAndInMemoryDbContext();
        var controller = new ItemController(repository);

        var itemCount = context.ItemTypes.Count();
        
        var result = await controller.DeleteConfirmed(1);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.ItemTypes.Count().Should().Be(itemCount - 1);
    }
}