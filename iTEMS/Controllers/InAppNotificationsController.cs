using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using iTEMS.Data;
using iTEMS.Models;

namespace iTEMS.Controllers
{
    public class InAppNotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InAppNotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InAppNotifications
        public async Task<IActionResult> Index()
        {
            return View(await _context.InAppNotification.ToListAsync());
        }

        // GET: InAppNotifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inAppNotification = await _context.InAppNotification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inAppNotification == null)
            {
                return NotFound();
            }

            return View(inAppNotification);
        }

        // GET: InAppNotifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InAppNotifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Message,Timestamp")] InAppNotification inAppNotification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inAppNotification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inAppNotification);
        }

        // GET: InAppNotifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inAppNotification = await _context.InAppNotification.FindAsync(id);
            if (inAppNotification == null)
            {
                return NotFound();
            }
            return View(inAppNotification);
        }

        // POST: InAppNotifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Message,Timestamp")] InAppNotification inAppNotification)
        {
            if (id != inAppNotification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inAppNotification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InAppNotificationExists(inAppNotification.Id))
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
            return View(inAppNotification);
        }

        // GET: InAppNotifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inAppNotification = await _context.InAppNotification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inAppNotification == null)
            {
                return NotFound();
            }

            return View(inAppNotification);
        }

        // POST: InAppNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inAppNotification = await _context.InAppNotification.FindAsync(id);
            if (inAppNotification != null)
            {
                _context.InAppNotification.Remove(inAppNotification);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InAppNotificationExists(int id)
        {
            return _context.InAppNotification.Any(e => e.Id == id);
        }
    }
}
