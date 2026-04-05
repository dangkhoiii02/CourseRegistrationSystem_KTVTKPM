using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public enum Gender
{
    Male = 0,
    Female = 1
}

public class User
{
    [Key]
    [MaxLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string RoleId { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string NationalId { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Address { get; set; } = string.Empty;

    // Navigation
    public Role Role { get; set; } = null!;
    public Student? Student { get; set; }
    public Lecturer? Lecturer { get; set; }
}
