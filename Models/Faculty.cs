using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class Faculty
{
    [Key]
    [MaxLength(20)]
    public string FacultyId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FacultyName { get; set; } = string.Empty;

    public ICollection<StudentClass> Classes { get; set; } = new List<StudentClass>();
    public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
