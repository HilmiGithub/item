using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iTEMS.ViewModels
{
    public class AssignRoleViewModel
    {
        public IdentityUser User { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public string SelectedRoleId { get; set; }
        public List<string> AssignedRoles { get; set; }
        public List<string> RoleNames { get; set; }
        public AssignRoleViewModel()
        {
            AssignedRoles = new List<string>();
        }


    }
}