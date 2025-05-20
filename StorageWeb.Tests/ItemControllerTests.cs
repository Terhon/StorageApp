using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Controllers;
using StorageWeb.Data;
using StorageWeb.Models;

namespace StorageApp.UnitTests;

public class ItemControllerTests
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
        context.Item.Add(new Item{ Id = 1, Name = "Item 1" , Unit = "unit 1"});
        context.Item.Add(new Item{ Id = 2, Name = "Item 2" , Unit = "unit 2"});
        
        context.SaveChanges();
    }
    
    [Fact]
    public async Task Index_ReturnsViewResult_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new ItemController(context);
        
        var result = await controller.Index();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Item>>(viewResult.ViewData.Model);
        model.Count().Should().Be(2);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new ItemController(context);
        
        var result = await controller.Details(1);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Item>(viewResult.ViewData.Model);
        model.Name.Should().Be("Item 1");
        model.Unit.Should().Be("unit 1");
    }

    [Fact]
    public async Task Create_AddsItemToItemController_WhenValid()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new ItemController(context);
        
        var newItem = new Item { Name = "New Item", Unit = "unit 1" };
        var result = await controller.Create(newItem);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.Item.Count().Should().Be(3);
        context.Item.Should().Contain(newItem);
    }
    
    [Fact]
    public async Task Create_AddsItemToItemController_WhenInvalid()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new ItemController(context);
        
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
        var context = GetInMemoryStorageWebContext();
        var controller = new ItemController(context);
        
        var result = await controller.Edit(1);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Item>(viewResult.ViewData.Model);
        model.Name.Should().Be("Item 1");
        model.Unit.Should().Be("unit 1");
    }

    [Fact]
    public async Task Delete_ReturnsViewResult_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new ItemController(context);
        
        var result = await controller.Delete(1);
        var viewResult = Assert.IsType<ViewResult>(result);
            
        var model = Assert.IsAssignableFrom<Item>(viewResult.ViewData.Model);
        model.Name.Should().Be("Item 1");
        model.Unit.Should().Be("unit 1");
    }

    [Fact]
    public void Create_ReturnsViewResult()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new ItemController(context);
        
        var result = controller.Create();
        
        Assert.IsType<ViewResult>(result);
    }
}