using System.ComponentModel.DataAnnotations;

namespace iTEMS.Models
{
    public class UserActivity
    {
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set;}
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set;}
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set;}
        [Display(Name = "Modified On")]
        public DateTime ModifiedOn { get; set;}
    }
}
