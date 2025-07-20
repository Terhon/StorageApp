using Microsoft.AspNetCore.Mvc;
using Storage.Application.Commands;
using Storage.Application.Interfaces;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class ItemController(IItemTypeRepository context) : Controller
    {
        // GET: Item
        public async Task<IActionResult> Index()
        {
            //return View(await context.Item.ToListAsync());
            return View();
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*
                if (id == null)
                {
                    return NotFound();
                }

                var item = await context.Item
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (item == null)
                {
                    return NotFound();
                }

                return View(item);*/
            return View();
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
            await context.AddItemType(new CreateItemTypeCommand { Name = item.Name, Unit = item.Unit });

            return RedirectToAction(nameof(Index));
        }

        // GET: Item/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*
                if (id == null)
                {
                    return NotFound();
                }

                var item = await context.Item.FindAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                return View(item);*/
            return View();
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(item);
            
            await context.UpdateItemType(new UpdateItemTypeCommand { Id = item.Id, Name = item.Name, Unit = item.Unit });
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Item/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*
                if (id == null)
                {
                    return NotFound();
                }

                var item = await context.Item
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (item == null)
                {
                    return NotFound();
                }

                return View(item);*/
            return View();
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await context.DeleteItemType(new DeleteItemTypeCommand(id));

            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            //return context.Item.Any(e => e.Id == id);
            return false;
        }
    }
}