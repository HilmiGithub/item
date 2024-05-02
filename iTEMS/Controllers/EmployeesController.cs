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
using Microsoft.AspNetCore.Identity;
using iTEMS.ViewModels;

namespace iTEMS.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            // Retrieve the current user
            var currentUser = await _userManager.GetUserAsync(User);

            // Retrieve the employee based on the current user's email
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == currentUser.UserName);

            // Check if an employee record with a matching email was found
            if (employee != null)
            {
                // If a matching employee record is found, display the first name
                ViewBag.DisplayName = employee.FirstName;
            }
            else
            {
                // If no matching employee record is found, display the email
                ViewBag.DisplayName = "Test";
            }
            return View(await _context.Employees.ToListAsync());
        }


        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == currentUser.UserName);

            if (employee != null)
            {
                ViewBag.FirstName = employee.FirstName;
            }
            else
            {
                ViewBag.FirstName = "Test";
            }

            return View();
        }


        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmpNo,FirstName,LastName,PhoneNumber,Email,Country,DateofBirth,Address,Department,Designation,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreatedOn = DateTime.Now;
                
                _context.Add(employee);
                await _context.SaveChangesAsync();

                // Assign the default role "User" to the newly created user
                var user = new IdentityUser { UserName = employee.Email, Email = employee.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User"); // Set the default role to "User"
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(employee);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmpNo,UserName,FirstName,LastName,PhoneNumber,Email,Country,DateofBirth,Address,Department,Designation,CreatedOn,ModifiedBy,ModifiedOn")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the current user
                    var currentUser = await _userManager.GetUserAsync(User);

                    // Assign the current user's username to the ModifiedBy property
                    employee.ModifiedBy = currentUser.UserName;

                    // Set the ModifiedOn property to the current date and time
                    employee.ModifiedOn = DateTime.Now;

                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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

            return View(employee);
        }


        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Employees/AssignRole
        public async Task<IActionResult> AssignRole(string userId = null)
        {
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                // Retrieve user's assigned roles
                var assignedRoles = await _userManager.GetRolesAsync(user);

                // Retrieve all users and roles
                var users = await _userManager.Users.ToListAsync();
                var roles = await _context.Roles.ToListAsync();

                // Pass users, roles, and assigned roles to the view
                ViewBag.Users = new SelectList(users, "Id", "UserName");
                ViewBag.Roles = new SelectList(roles, "Name", "Name");

                var viewModel = new AssignRoleViewModel(); // Instantiate your ViewModel
                return View(viewModel);
            }
            else
            {
                // Retrieve all users and roles
                var users = await _userManager.Users.ToListAsync();
                var roles = await _context.Roles.ToListAsync();

                // Pass users and roles to the view
                ViewBag.Users = new SelectList(users, "Id", "UserName");
                ViewBag.Roles = new SelectList(roles, "Name", "Name");

                var viewModel = new AssignRoleViewModel(); // Instantiate your ViewModel
                return View(viewModel);
            }
        }


        // POST: Employees/AssignRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Invalid user ID or role name.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);


            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                // Redirect to the index page or any other page
                return RedirectToAction(nameof(Index));
            }

            // If there are errors, return to the AssignRole page with error messages
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // Pass the view model to the view
            var users = await _userManager.Users.ToListAsync();
            var roles = await _context.Roles.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View();
        }





        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
