using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class Semester
{
    [Key]
    [MaxLength(20)]
    public string SemesterId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string SemesterName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string AcademicYear { get; set; } = string.Empty;

    // Navigation
    public ICollection<Section> Sections { get; set; } = new List<Section>();
    public ICollection<RegistrationPeriod> RegistrationPeriods { get; set; } = new List<RegistrationPeriod>();
}
