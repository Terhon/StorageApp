using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.Application.ItemType.Queries;
using Storage.Application.StorageItem.Commands;
using Storage.Application.StorageItem.Commands.DTOs;
using Storage.Application.StorageItem.Queries;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class StorageItemController(
        IStorageItemCommandService commandService,
        IStorageItemQueryService queryService,
        IItemTypeQueryService itemTypeQueryService) : Controller
    {
        // GET: StorageItem
        public async Task<IActionResult> Index()
        {
            //var storageWebContext = context.StorageItems.Include(s => s.Item);
            var storageItems = await queryService.GetAllStorageItems();
            return View(storageItems.Select(s => s.Map()).ToList());
        }

        // GET: StorageItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await queryService.GetStorageItemById(id.Value);
            if (storageItem == null)
            {
                return NotFound();
            }

            return View(storageItem.Map());
        }

        // GET: StorageItem/Create
        public async Task<IActionResult> Create()
        {
            var itemTypes = await itemTypeQueryService.GetAllItemTypes();
            ViewData["ItemId"] = new SelectList(itemTypes, "Id", "Name");
            return View();
        }

        // POST: StorageItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StorageItem storageItem)
        {
            if (ModelState.IsValid)
            {
                await commandService.AddStorageItem(new CreateStorageItemCommand
                {
                    ItemTypeId = storageItem.ItemId,
                    AcquisitionDate = storageItem.AcquisitionDate,
                    Amount = storageItem.Amount
                });
                return RedirectToAction(nameof(Index));
            }

            var itemTypes = await itemTypeQueryService.GetAllItemTypes();
            ViewData["ItemId"] = new SelectList(itemTypes, "Id", "Name", storageItem.ItemId);
            return View(storageItem);
        }

        // GET: StorageItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await queryService.GetStorageItemById(id.Value);
            if (storageItem == null)
            {
                return NotFound();
            }
            var item = storageItem.Map();
            
            var itemTypes = await itemTypeQueryService.GetAllItemTypes();
            ViewData["ItemId"] = new SelectList(itemTypes, "Id", "Name", item.ItemId);
            return View(item);
        }

        // POST: StorageItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StorageItem storageItem)
        {
            if (id != storageItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await commandService.UpdateStorageItem(new UpdateStorageItemCommand
                    {
                        Id = storageItem.Id,
                        AcquisitionDate = storageItem.AcquisitionDate,
                        Amount = storageItem.Amount,
                        ItemTypeId = storageItem.ItemId
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StorageItemExists(storageItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            
            var itemTypes = await itemTypeQueryService.GetAllItemTypes();
            
            ViewData["ItemId"] = new SelectList(itemTypes, "Id", "Name", storageItem.ItemId);
            return View(storageItem);
        }

        // GET: StorageItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await queryService.GetStorageItemById(id.Value); 
            /*context.StorageItems
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.Id == id);*/
            if (storageItem == null)
            {
                return NotFound();
            }

            return View(storageItem.Map());
        }

        // POST: StorageItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storageItem = await queryService.GetStorageItemById(id);
            if (storageItem != null)
            {
                await commandService.DeleteStorageItem(new DeleteStorageItemCommand(id));
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StorageItemExists(int id)
        {
            var storageItems = await queryService.GetAllStorageItems();
            return storageItems.Any(e => e.Id == id);
        }
    }
}