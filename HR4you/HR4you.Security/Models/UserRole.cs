using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR4you.Security.Models
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        
        public bool IsSpecialRole { get; set; }
    }
}