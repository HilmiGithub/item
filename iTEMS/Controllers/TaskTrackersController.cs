using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using iTEMS.Data;
using iTEMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using static iTEMS.Models.TaskTracker;

namespace iTEMS.Controllers
{
    [Authorize]
    public class TaskTrackersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TaskTrackersController> _logger; // Injected logger
        private readonly UserManager<IdentityUser> _userManager;

        public TaskTrackersController(ApplicationDbContext context, ILogger<TaskTrackersController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: TaskTrackers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TaskTrackers
                .Include(t => t.Project)
                .Include(t => t.Employee); // Include the Employee navigation property
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: TaskTrackers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTracker = await _context.TaskTrackers
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskTracker == null)
            {
                return NotFound();
            }

            return View(taskTracker);
        }

        // GET: TaskTrackers/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name");
            ViewData["AssignedTo"] = new SelectList(_context.Employees, "Id", "FirstName");
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(TaskTrackerStatus)));
            return View();
        }

        // POST: TaskTrackers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,AssignedTo,Status,Priority,DueDate,StartDate,EstimatedTime,ActualTime,Tags,Attachments,Comments,ProjectId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] TaskTracker taskTracker)
        {
            try
            {
                    var currentUser = await _userManager.GetUserAsync(User);
                    taskTracker.CreatedBy = currentUser.UserName;
                    taskTracker.CreatedOn = DateTime.Now; // Set the creation date here
                    taskTracker.ModifiedBy = currentUser.UserName;
                    taskTracker.ModifiedOn = DateTime.Now;
                    _context.Add(taskTracker);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while creating a TaskTracker.");

                // Optionally, you can add the exception message to the ModelState
                ModelState.AddModelError("", "An error occurred while saving the task.");
            }

            // If ModelState is invalid or an exception occurred, return to the Create view with error messages
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", taskTracker.ProjectId);
            ViewData["AssignedTo"] = new SelectList(_context.Employees, "Id", "FirstName",taskTracker.Employee);
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(TaskTrackerStatus)),taskTracker.Status);
            return View(taskTracker);
        }


        // GET: TaskTrackers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTracker = await _context.TaskTrackers.FindAsync(id);
            if (taskTracker == null)
            {
                return NotFound();
            }

            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", taskTracker.ProjectId);
            ViewData["AssignedTo"] = new SelectList(_context.Employees, "Id", "FirstName", taskTracker.Employee);
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(TaskTrackerStatus)), taskTracker.Status);
            return View(taskTracker);
        }




        // POST: TaskTrackers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,AssignedTo,Status,Priority,DueDate,StartDate,EstimatedTime,ActualTime,Tags,Attachments,Comments,ProjectId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] TaskTracker taskTracker)
        {
            if (id != taskTracker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);

                    // Assign the current user's username to the ModifiedBy property
                    taskTracker.ModifiedBy = currentUser.UserName;

                    // Set the ModifiedOn property to the current date and time
                    taskTracker.ModifiedOn = DateTime.Now;

                    _context.Update(taskTracker);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTrackerExists(taskTracker.Id))
                    {
                        ModelState.AddModelError("", "An error occurred while saving the task. Task does not exist.");
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while saving the task.");
                        throw;
                    }
                }
                // Handle other specific exceptions if needed
            }
            else
            {
                // If ModelState is not valid, return to the Edit view with error messages
                ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", taskTracker.ProjectId);
                ViewData["AssignedTo"] = new SelectList(_context.Employees, "Id", "FirstName", taskTracker.Employee);
                ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(TaskTrackerStatus)), taskTracker.Status);

                // Find the first error in ModelState and add an error message indicating the unfilled field
                var firstError = ModelState.Values.FirstOrDefault(v => v.Errors.Any());
                if (firstError != null)
                {
                    var unfilledField = firstError.Errors.FirstOrDefault()?.ErrorMessage;
                    ModelState.AddModelError("", $"{unfilledField}");
                }
                else
                {
                    ModelState.AddModelError("", "Please fill out all required fields.");
                }

                return View(taskTracker);
            }


        }

        // GET: TaskTrackers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTracker = await _context.TaskTrackers
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskTracker == null)
            {
                return NotFound();
            }

            return View(taskTracker);
        }

        // POST: TaskTrackers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskTracker = await _context.TaskTrackers.FindAsync(id);
            if (taskTracker != null)
            {
                _context.TaskTrackers.Remove(taskTracker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskTrackerExists(int id)
        {
            return _context.TaskTrackers.Any(e => e.Id == id);
        }
    }
}
