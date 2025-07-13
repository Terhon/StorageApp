using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Data;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class StorageItemController(StorageWebContext context) : Controller
    {
        // GET: StorageItem
        public async Task<IActionResult> Index()
        {
            var storageWebContext = context.StorageItems.Include(s => s.Item);
            return View(await storageWebContext.ToListAsync());
        }

        // GET: StorageItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await context.StorageItems
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storageItem == null)
            {
                return NotFound();
            }

            return View(storageItem);
        }

        // GET: StorageItem/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(context.Item, "Id", "Name");
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
                context.Add(storageItem);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(context.Item, "Id", "Name", storageItem.ItemId);
            return View(storageItem);
        }

        // GET: StorageItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await context.StorageItems.FindAsync(id);
            if (storageItem == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(context.Item, "Id", "Name", storageItem.ItemId);
            return View(storageItem);
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
                    context.Update(storageItem);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StorageItemExists(storageItem.Id))
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
            ViewData["ItemId"] = new SelectList(context.Item, "Id", "Name", storageItem.ItemId);
            return View(storageItem);
        }

        // GET: StorageItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await context.StorageItems
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storageItem == null)
            {
                return NotFound();
            }

            return View(storageItem);
        }

        // POST: StorageItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storageItem = await context.StorageItems.FindAsync(id);
            if (storageItem != null)
            {
                context.StorageItems.Remove(storageItem);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorageItemExists(int id)
        {
            return context.StorageItems.Any(e => e.Id == id);
        }
    }
}
