using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Data;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class StorageItemController : Controller
    {
        private readonly StorageWebContext _context;

        public StorageItemController(StorageWebContext context)
        {
            _context = context;
        }

        // GET: StorageItem
        public async Task<IActionResult> Index()
        {
            var storageWebContext = _context.StorageItems.Include(s => s.Item);
            return View(await storageWebContext.ToListAsync());
        }

        // GET: StorageItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await _context.StorageItems
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
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name");
            return View();
        }

        // POST: StorageItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,AcquisitionDate,Amount")] StorageItem storageItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storageItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", storageItem.ItemId);
            return View(storageItem);
        }

        // GET: StorageItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await _context.StorageItems.FindAsync(id);
            if (storageItem == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", storageItem.ItemId);
            return View(storageItem);
        }

        // POST: StorageItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,AcquisitionDate,Amount")] StorageItem storageItem)
        {
            if (id != storageItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storageItem);
                    await _context.SaveChangesAsync();
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
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", storageItem.ItemId);
            return View(storageItem);
        }

        // GET: StorageItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageItem = await _context.StorageItems
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
            var storageItem = await _context.StorageItems.FindAsync(id);
            if (storageItem != null)
            {
                _context.StorageItems.Remove(storageItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorageItemExists(int id)
        {
            return _context.StorageItems.Any(e => e.Id == id);
        }
    }
}
