using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR4you.Security.Models;

[Table("hr4you_userRole")]
public class UserRole
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string AuthScheme { get; set; } = string.Empty;
    public bool IsSpecialRole { get; set; }
}