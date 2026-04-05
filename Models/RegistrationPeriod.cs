using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class RegistrationPeriod
{
    [Key]
    [MaxLength(30)]
    public string PeriodId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string PeriodName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string SemesterId { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [MaxLength(50)]
    public string TargetAudience { get; set; } = string.Empty;

    // Navigation
    public Semester Semester { get; set; } = null!;
}
