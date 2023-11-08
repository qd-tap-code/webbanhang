using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace qlthucung.Models
{
    public class UserRole
    {
        public AspNetUser User { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}
