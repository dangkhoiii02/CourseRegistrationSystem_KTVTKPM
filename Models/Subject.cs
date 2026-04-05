using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class Subject
{
    [Key]
    [MaxLength(20)]
    public string SubjectId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string SubjectName { get; set; } = string.Empty;

    public int Credits { get; set; }

    [Required]
    [MaxLength(20)]
    public string FacultyId { get; set; } = string.Empty;

    // Navigation
    public Faculty Faculty { get; set; } = null!;
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}
