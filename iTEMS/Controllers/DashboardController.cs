using iTEMS.Data;
using iTEMS.Models;
using iTEMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            

            TotalProjects = _context.Project.Count(),
            PlanningProjects = _context.Project.Count(p => p.Status == ProjectStatus.Planning),
            PendingProjects = _context.Project.Count(p => p.Status == ProjectStatus.Pending),
            DelayedProjects = _context.Project.Count(p => p.Status == ProjectStatus.Delayed),
            BlockedProjects = _context.Project.Count(p => p.Status == ProjectStatus.Blocked),
            CompletedProjects = _context.Project.Count(p => p.Status == ProjectStatus.Completed),
            ActiveProjectsList = _context.Project.Where(p => p.Status == ProjectStatus.Planning || p.Status == ProjectStatus.Pending || p.Status == ProjectStatus.Delayed || p.Status == ProjectStatus.Blocked).ToList(),
            TeamMembers = _context.Employees.ToList(),
            TotalTasks = _context.TaskTrackers.Count(),
            PlanningTasks = _context.TaskTrackers.Count(t => t.Status == "Planning"),
            PendingTasks = _context.TaskTrackers.Count(t => t.Status == "Pending"),
            DelayedTasks = _context.TaskTrackers.Count(t => t.Status == "Delayed"),
            BlockedTasks = _context.TaskTrackers.Count(t => t.Status == "Blocked"),
            CompletedTasks = _context.TaskTrackers.Count(t => t.Status == "Completed"),
            ActiveTasksList = _context.TaskTrackers.Where(t => t.Status == "Planning" || t.Status == "Pending" || t.Status == "Delayed" || t.Status == "Blocked").ToList(),

            TeamMembersProjects = new Dictionary<Employee, List<Project>>(),
            TeamMembersTasks = new Dictionary<Employee, List<TaskTracker>>()
        };

        foreach (var member in viewModel.TeamMembers)
        {
            // Get projects for each team member
            var projects = await _context.Project
                .Where(p => p.Tasks.Any(t => t.AssignedTo == member.Id))
                .ToListAsync();

            // Get tasks for each team member
            var tasks = await _context.TaskTrackers
                .Where(t => t.AssignedTo == member.Id)
                .ToListAsync();

            // Add to the dictionaries
            viewModel.TeamMembersProjects.Add(member, projects);
            viewModel.TeamMembersTasks.Add(member, tasks);
        }


        return View(viewModel);
    }
}
