using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Storage.Application.ItemType.Queries;
using Storage.Application.StorageItem.Commands;
using Storage.Application.StorageItem.Queries;
using Storage.Application.StorageItem.Queries.DTOs;
using Storage.Domain.Entities;
using Storage.Infrastructure.Data;
using Storage.Infrastructure.Services.ItemType;
using Storage.Infrastructure.Services.StorageItem;
using StorageWeb.Controllers;
using StorageWeb.Models;
using StorageItem = Storage.Domain.Entities.StorageItem;

namespace StorageApp.UnitTests;

public class StorageItemControllerTests
{
    private (IStorageItemCommandService, IStorageItemQueryService, IItemTypeQueryService, StorageDbContext) GetInMemoryStorageWebContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<StorageDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new StorageDbContext(options);
        context.Database.EnsureCreated();

        Seed(context);

        return (new StorageItemCommandService(context), new StorageItemQueryService(context), new ItemTypeQueryService(context), context);
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

        context.StorageItems.Add(new StorageItem
            { ItemType = itemTypes[0], Amount = 3, AcquisitionDate = DateTime.Now.AddDays(-1) });
        context.StorageItems.Add(new StorageItem
            { ItemType = itemTypes[0], Amount = 6, AcquisitionDate = DateTime.Now.AddDays(-5) });
        context.StorageItems.Add(
            new StorageItem { ItemType = itemTypes[1], Amount = 20, AcquisitionDate = DateTime.Now.AddDays(-3) });

        context.SaveChanges();
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WhenItemExists()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<StorageWeb.Models.StorageItem>>(viewResult.ViewData.Model);
        model.Count().Should().Be(3);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WhenItemDoesNotExist()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var storageItem = context.StorageItems
            .Include(storageItem => storageItem.ItemType)
            .Single(s => s.Id == 1);

        var result = await controller.Details(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<StorageWeb.Models.StorageItem>(viewResult.ViewData.Model);
        model.Item.Should().BeEquivalentTo(new Item{Id = storageItem.ItemType.Id, Name = storageItem.ItemType.Name, Unit = storageItem.ItemType.Unit});
        model.Amount.Should().Be(storageItem.Amount);
        model.AcquisitionDate.Should().Be(storageItem.AcquisitionDate);
    }

    [Fact]
    public void Create_ReturnsViewResult()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var result = controller.Create();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_AddsItemToItemController_WhenValid()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var newItem = new StorageWeb.Models.StorageItem
            { ItemId = context.ItemTypes.First().Id, Amount = 100, AcquisitionDate = DateTime.Now };
        var result = await controller.Create(newItem);

        Assert.IsType<RedirectToActionResult>(result);
        context.StorageItems.Count().Should().Be(4);
        context.StorageItems.Should().ContainSingle(i => i.AcquisitionDate == newItem.AcquisitionDate
            && i.Amount == newItem.Amount && i.ItemType.Id == newItem.ItemId);
    }

    [Fact]
    public async Task Edit_ReturnsViewResult_WhenItemExists()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var storageItem = context.StorageItems
            .Include(storageItem => storageItem.ItemType)
            .Single(s => s.Id == 1);

        var result = await controller.Edit(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<StorageWeb.Models.StorageItem>(viewResult.ViewData.Model);
        model.Item.Should().BeEquivalentTo(new Item{Id = storageItem.ItemType.Id, Name = storageItem.ItemType.Name, Unit = storageItem.ItemType.Unit});
        model.Amount.Should().Be(storageItem.Amount);
        model.AcquisitionDate.Should().Be(storageItem.AcquisitionDate);
    }

    [Fact]
    public async Task Edit_ReturnsNotFoundResult_WhenNullId()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var result = await controller.Edit(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ModifiesItem_WhenItemExists()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var newAmount = 1234;
        var newDate = DateTime.Now;

        var editedItem = context.StorageItems.First();
        editedItem.Amount = newAmount;
        editedItem.AcquisitionDate = newDate;
        var result = await controller.Edit(editedItem.Id, new StorageWeb.Models.StorageItem
        {
            Id = editedItem.Id,
            Amount = newAmount,
            AcquisitionDate = newDate,
            ItemId = editedItem.ItemType.Id
        });

        Assert.IsType<RedirectToActionResult>(result);
        var item = context.StorageItems.Single(x => x.Id == editedItem.Id);
        item.Should().NotBeNull();
        item.Amount.Should().Be(newAmount);
        item.AcquisitionDate.Should().Be(newDate);
    }

    [Fact]
    public async Task Delete_ReturnsViewResult_WhenItemExists()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var storageItem = context.StorageItems
            .Include(storageItem => storageItem.ItemType)
            .Single(s => s.Id == 1);

        var result = await controller.Delete(storageItem.Id);
        var viewResult = Assert.IsType<ViewResult>(result);

        var model = Assert.IsAssignableFrom<StorageWeb.Models.StorageItem>(viewResult.ViewData.Model);
        storageItem.Should().NotBeNull();
        storageItem.Amount.Should().Be(storageItem.Amount);
        storageItem.AcquisitionDate.Should().Be(storageItem.AcquisitionDate);
    }

    [Fact]
    public async Task DeleteConfirmed_RemovesItem_WhenItemExists()
    {
        var (commandService, queryService, itemTypeQueryService, context) = GetInMemoryStorageWebContext();
        var controller = new StorageItemController(commandService, queryService, itemTypeQueryService);

        var itemCount = context.StorageItems.Count();

        var result = await controller.DeleteConfirmed(1);

        Assert.IsType<RedirectToActionResult>(result);
        context.StorageItems.Count().Should().Be(itemCount - 1);
    }
}