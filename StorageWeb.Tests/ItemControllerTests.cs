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
    public async Task Details_ReturnsViewResult()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new ItemController(context);
        
        var result = await controller.Details(1);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Item>(viewResult.ViewData.Model);
        model.Name.Should().Be("Item 1");
        model.Unit.Should().Be("unit 1");
    }
}