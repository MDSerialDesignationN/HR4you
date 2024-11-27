using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR4you.Security.Models
{
    [Table("UserRoleGroup")]
    public class UserRoleGroup
    {
        [Key]
        public required string Name { get; set; }
        public string[]? Roles { get; set; }
        public List<User>? UserIds { get; set; }
    }
}