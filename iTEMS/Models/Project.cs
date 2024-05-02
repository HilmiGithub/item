using System;
using System.ComponentModel.DataAnnotations;

namespace iTEMS.Models
{
    public class Project : UserActivity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name for the project.")]
        [StringLength(100, ErrorMessage = "The project name must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The description cannot exceed {1} characters.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Please select a status for the project.")]
        public ProjectStatus Status { get; set; }

        public string? PIC { get; set; }
        public string? Budget { get; set; }
        public string? Update { get; set; }
        public string? Blocker { get; set; }


        public ICollection<TaskTracker>? Tasks { get; set; }
    }

    public enum ProjectStatus
    {
        Planning,
        Pending,
        Completed,
        Delayed,
        Blocked
    }
}
