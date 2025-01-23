using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HR4you.Security.Models
{
    [Table("hr4you_user")]
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AuthToken { get; set; } 
        public bool IsDeleted { get; set; }
        public List<UserRoleGroup>? RoleGroups { get; set; }
        
        public List<string> GetRoles()
        {
            var result = new List<string>();
            if (RoleGroups == null)
            {
                return result;
            }
            foreach (var group in RoleGroups)
            {
                result.AddRange(group.Roles);
            }
            return result.Distinct().ToList();
        }
    }
}