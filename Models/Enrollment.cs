using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public enum EnrollmentStatus
{
    Active,
    Cancelled
}

public class Enrollment
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string SectionId { get; set; } = string.Empty;

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    // Navigation
    public Student Student { get; set; } = null!;
    public Section Section { get; set; } = null!;
}
