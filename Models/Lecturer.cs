using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.Models;

public class Lecturer
{
    [Key]
    [MaxLength(20)]
    public string LecturerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string FacultyId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Degree { get; set; } = string.Empty;

    // Navigation
    public User User { get; set; } = null!;
    public Faculty Faculty { get; set; } = null!;
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}
