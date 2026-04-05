using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class Student
{
    [Key]
    [MaxLength(20)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string ClassId { get; set; } = string.Empty;

    // Navigation
    public User User { get; set; } = null!;
    public StudentClass Class { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
