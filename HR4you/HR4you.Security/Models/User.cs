using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HR4you.Security.Models
{
    public class User : IdentityUser
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
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