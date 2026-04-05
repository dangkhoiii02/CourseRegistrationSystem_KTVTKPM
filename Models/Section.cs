using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class Section
{
    [Key]
    [MaxLength(30)]
    public string SectionId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string SubjectId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string SemesterId { get; set; } = string.Empty;

    public int GroupNumber { get; set; }

    [Required]
    [MaxLength(20)]
    public string LecturerId { get; set; } = string.Empty;

    public int MaxCapacity { get; set; }

    public int RegisteredCount { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    // Navigation
    public Subject Subject { get; set; } = null!;
    public Semester Semester { get; set; } = null!;
    public Lecturer Lecturer { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
