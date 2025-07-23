using Microsoft.AspNetCore.Mvc;
using Storage.Application.ItemType.Commands;
using Storage.Application.ItemType.Commands.DTOs;
using Storage.Application.ItemType.Queries;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class ItemController(IItemTypeCommandService commandService, IItemTypeQueryService queryService) : Controller
    {
        // GET: Item
        public async Task<IActionResult> Index()
        {
            var itemTypes = await queryService.GetAllItemTypes();
            var items = itemTypes.Select(i => i.Map());
            return View(items);
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await queryService.GetItemTypeById(id.Value);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType.Map());
        }

        // GET: Item/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            if (!ModelState.IsValid)
                return View(item);
            await commandService.AddItemType(new CreateItemTypeCommand { Name = item.Name, Unit = item.Unit });

            return RedirectToAction(nameof(Index));
        }

        // GET: Item/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await queryService.GetItemTypeById(id.Value);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType.Map());
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(item);

            await commandService.UpdateItemType(new UpdateItemTypeCommand
                { Id = item.Id, Name = item.Name, Unit = item.Unit });

            return RedirectToAction(nameof(Index));
        }

        // GET: Item/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
                if (id == null)
                {
                    return NotFound();
                }

                var itemType = await queryService.GetItemTypeById(id.Value);
                if (itemType == null)
                {
                    return NotFound();
                }
                
                return View(itemType.Map());
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await commandService.DeleteItemType(new DeleteItemTypeCommand(id));

            return RedirectToAction(nameof(Index));
        }
    }
}