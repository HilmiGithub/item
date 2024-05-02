using iTEMS.Models;
using System.ComponentModel.DataAnnotations;

namespace iTEMS.ViewModels
{
    public class RolesViewModel : UserActivity
    {
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }


    }
}
