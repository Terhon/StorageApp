using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Controllers;
using StorageWeb.Data;
using StorageWeb.Models;

namespace StorageApp.UnitTests;

public class StorageItemControllerTests
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

        context.StorageItems.Add(new StorageItem { ItemId = 1, Amount = 3, AcquisitionDate = DateTime.Now.AddDays(-1) });
        context.StorageItems.Add(new StorageItem { ItemId = 1, Amount = 6, AcquisitionDate = DateTime.Now.AddDays(-5) });
        context.StorageItems.Add(new StorageItem { ItemId = 2, Amount = 20, AcquisitionDate = DateTime.Now.AddDays(-3) });
        
        context.SaveChanges();
    }
    
    [Fact]
    public async Task Index_ReturnsViewResult_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);
        
        var result = await controller.Index();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<StorageItem>>(viewResult.ViewData.Model);
        model.Count().Should().Be(3);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WhenItemDoesNotExist()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);
        
        var storageItem = context.StorageItems
            .Include(storageItem => storageItem.Item)
            .Single(s => s.Id == 1);
        
        var result = await controller.Details(1);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<StorageItem>(viewResult.ViewData.Model);
        model.Item.Should().Be(storageItem.Item);
        model.Amount.Should().Be(storageItem.Amount);
        model.AcquisitionDate.Should().Be(storageItem.AcquisitionDate);
    }
    
    [Fact]
    public void Create_ReturnsViewResult()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);
        
        var result = controller.Create();
        
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async Task Create_AddsItemToItemController_WhenValid()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);
        
        var newItem = new StorageItem { ItemId = context.Item.First().Id, Amount = 100, AcquisitionDate = DateTime.Now };
        var result = await controller.Create(newItem);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.StorageItems.Count().Should().Be(4);
        context.StorageItems.Should().Contain(newItem);
    }
    
    [Fact]
    public async Task Edit_ReturnsViewResult_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);
        
        var storageItem = context.StorageItems
            .Include(storageItem => storageItem.Item)
            .Single(s => s.Id == 1);
        
        var result = await controller.Edit(1);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<StorageItem>(viewResult.ViewData.Model);
        model.Item.Should().Be(storageItem.Item);
        model.Amount.Should().Be(storageItem.Amount);
        model.AcquisitionDate.Should().Be(storageItem.AcquisitionDate);
    }
    
    [Fact]
    public async Task Edit_ReturnsNotFoundResult_WhenNullId()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);
        
        var result = await controller.Edit(null);
        
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Edit_ModifiesItem_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);
        
        var newAmount = 1234;
        var newDate = DateTime.Now;
        
        var editedItem = context.StorageItems.First();
        editedItem.Amount = newAmount;
        editedItem.AcquisitionDate = newDate;
        var result = await controller.Edit(editedItem.Id, editedItem);
        
        Assert.IsType<RedirectToActionResult>(result);
        var item = context.StorageItems.Single(x => x.Id == editedItem.Id);
        item.Should().NotBeNull();
        item.Amount.Should().Be(newAmount);
        item.AcquisitionDate.Should().Be(newDate);
    }
    
    [Fact]
    public async Task Delete_ReturnsViewResult_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);
        
        var storageItem = context.StorageItems
            .Include(storageItem => storageItem.Item)
            .Single(s => s.Id == 1);
        
        var result = await controller.Delete(storageItem.Id);
        var viewResult = Assert.IsType<ViewResult>(result);
            
        var model = Assert.IsAssignableFrom<StorageItem>(viewResult.ViewData.Model);
        storageItem.Should().NotBeNull();
        storageItem.Amount.Should().Be(storageItem.Amount);
        storageItem.AcquisitionDate.Should().Be(storageItem.AcquisitionDate);
    }
    
    [Fact]
    public async Task DeleteConfirmed_RemovesItem_WhenItemExists()
    {
        var context = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(context);

        var itemCount = context.StorageItems.Count();
        
        var result = await controller.DeleteConfirmed(1);
        
        Assert.IsType<RedirectToActionResult>(result);
        context.StorageItems.Count().Should().Be(itemCount - 1);
    }
}