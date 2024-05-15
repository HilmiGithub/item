namespace iTEMS.Models
{
    public class ProjectTaskDetails
    {
        public Project Project { get; set; }
        public List<TaskTracker> Tasks { get; set; }

        // Properties to hold task counts
        public int PlanningTasks { get; set; }
        public int PendingTasks { get; set; }
        public int DelayedTasks { get; set; }
        public int BlockedTasks { get; set; }
        public int CompletedTasks { get; set; }
    }

}
