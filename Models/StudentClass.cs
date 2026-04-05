using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegistrationSystem.Models;

[Table("Classes")]
public class StudentClass
{
    [Key]
    [MaxLength(30)]
    public string ClassId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ClassName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string FacultyId { get; set; } = string.Empty;

    [MaxLength(20)]
    public string AcademicYear { get; set; } = string.Empty;

    [MaxLength(50)]
    public string EducationType { get; set; } = string.Empty;

    // Navigation
    public Faculty Faculty { get; set; } = null!;
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
