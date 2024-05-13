using System.Collections.Generic;
using iTEMS.Models; // Assuming TaskTracker and Notification classes are in this namespace

namespace iTEMS.ViewModels
{
    public class TaskTrackerViewModel
    {
        public List<TaskTracker> TaskTrackers { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}
