using iTEMS.Models;

namespace iTEMS.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalTasks { get; set; }
        public int PlanningTasks { get; set; }
        public int PendingTasks { get; set; }
        public int DelayedTasks { get; set; }
        public int BlockedTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalProjects { get; set; }
        public int PlanningProjects { get; set; }
        public int PendingProjects { get; set; }
        public int DelayedProjects { get; set; }
        public int BlockedProjects { get; set; }
        public int CompletedProjects { get; set; }

        public List<Project> ActiveProjectsList { get; set; }
        public List<TaskTracker> ActiveTasksList { get; set; }

        public List<Employee> TeamMembers { get; set; }
        public Dictionary<Employee, List<Project>> TeamMembersProjects { get; set; }
        public Dictionary<Employee, List<TaskTracker>> TeamMembersTasks { get; set; }


        // Additional properties or collections as needed
    }

}
