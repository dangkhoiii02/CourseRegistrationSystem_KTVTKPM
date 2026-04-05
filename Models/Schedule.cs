using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class Schedule
{
    [Key]
    [MaxLength(20)]
    public string ScheduleId { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string SectionId { get; set; } = string.Empty;

    public int DayOfWeek { get; set; }

    public int StartPeriod { get; set; }

    public int PeriodCount { get; set; }

    [MaxLength(20)]
    public string Room { get; set; } = string.Empty;

    // Navigation
    public Section Section { get; set; } = null!;
}
