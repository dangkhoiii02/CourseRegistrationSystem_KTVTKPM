using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class Role
{
    [Key]
    [MaxLength(20)]
    public string RoleId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
}
