using System;
using System.Linq;
using System.Threading.Tasks;
using iTEMS.Data;
using iTEMS.Models;
using iTEMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace iTEMS.Controllers
{
    [Authorize]
    public class RolesController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            await SetNotificationsInViewBag();
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await SetNotificationsInViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolesViewModel model)
        {
            await SetNotificationsInViewBag();
            if (ModelState.IsValid)
            {
                // Create a new IdentityRole object with the provided role name
                var role = new IdentityRole { Name = model.RoleName };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            // If model state is invalid or role creation fails, return the same view with validation errors
            return View(model);
        }




        // GET: Roles/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            await SetNotificationsInViewBag();
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var model = new RolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RolesViewModel model)
        {
            await SetNotificationsInViewBag();
            if (id != model.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role == null)
                    {
                        return NotFound();
                    }

                    role.Name = model.RoleName;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(model.RoleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(model);
        }



        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            await SetNotificationsInViewBag();
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var model = new RolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await SetNotificationsInViewBag();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // If deletion fails, return to the delete confirmation page with the same model
            return View(new RolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            });
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            await SetNotificationsInViewBag();
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Fetch all users
            var users = await _userManager.Users.ToListAsync();

            // Create a SelectList with users' IDs as values and usernames as text
            var usersSelectList = new SelectList(users, "Id", "UserName");

            // Fetch all available roles
            var roles = await _roleManager.Roles.ToListAsync();

            var model = new AssignRoleViewModel
            {
                User = user, // Set the User property here
                Roles = roles,
                // Set a default selected role (optional)
                SelectedRoleId = roles.FirstOrDefault()?.Id
            };

            ViewBag.Users = usersSelectList; // Assign the SelectList to ViewBag.Users

            return View("~/Views/Employees/UserRoles.cshtml", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // AssignRole method
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            await SetNotificationsInViewBag();
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("UserId", "Invalid user ID");
                    return View(model);
                }

                var role = await _roleManager.FindByNameAsync(model.RoleName);
                if (role == null)
                {
                    ModelState.AddModelError("RoleName", "Invalid role name");
                    return View(model);
                }

                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserRoles", "Roles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }




        public IActionResult UserRoles()
        {

            // Query the AspNetUserRoles table to fetch user-role associations
            var userRoles = _context.UserRoles.ToList();

            // Create a dictionary to store user-role mappings
            var userRoleMap = new Dictionary<string, string>();

            // Populate the dictionary with user-role mappings
            foreach (var ur in userRoles)
            {
                // If the user is already in the dictionary, skip
                if (userRoleMap.ContainsKey(ur.UserId))
                {
                    continue;
                }

                // Otherwise, add the user-role mapping to the dictionary
                userRoleMap.Add(ur.UserId, _context.Roles.FirstOrDefault(r => r.Id == ur.RoleId)?.Name);
            }

            // Convert the dictionary to a list of AssignRoleViewModel objects
            var userRolesViewModels = userRoleMap.Select(kv => new AssignRoleViewModel
            {
                UserId = kv.Key,
                UserName = _context.Users.FirstOrDefault(u => u.Id == kv.Key)?.UserName,
                RoleName = kv.Value,
                Roles = _context.Roles.ToList(), // Populate the Roles property with the list of available roles
                                                 // Other properties initialization if necessary
            }).ToList();

            return View(userRolesViewModels);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(string userId, string selectedRoleId)
        {
            await SetNotificationsInViewBag();
            // Log the received user ID and role ID
            Console.WriteLine($"Received user ID: {userId}, role ID: {selectedRoleId}");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Log if the user is not found
                Console.WriteLine($"User with ID {userId} not found");
                return NotFound();
            }

            // Log the user's username
            Console.WriteLine($"Found user: {user.UserName}");

            var role = await _roleManager.FindByIdAsync(selectedRoleId);
            if (role == null)
            {
                // Log if the role is not found
                Console.WriteLine($"Role with ID {selectedRoleId} not found");
                return NotFound();
            }

            // Log the role name
            Console.WriteLine($"Found role: {role.Name}");

            // Remove existing roles
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Log the removed roles
            Console.WriteLine($"Removed existing roles: {string.Join(", ", userRoles)}");

            // Assign new role
            var result = await _userManager.AddToRoleAsync(user, role.Id);
            if (result.Succeeded)
            {
                // Log if the role assignment is successful
                Console.WriteLine($"Assigned user {user.UserName} to role {role.Name}");
                return RedirectToAction("UserRoles", "Roles");
            }
            else
            {
                // Log if there's an error during role assignment
                Console.WriteLine("Error assigning role:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                    ModelState.AddModelError("", error.Description);
                }
                return View("~/Views/Employees/AssignRole.cshtml");
            }
        }


        private bool RoleExists(string id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }

    }
}
