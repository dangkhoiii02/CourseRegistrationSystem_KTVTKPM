using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.DTOs;

public class RegisterEnrollmentDto
{
    [Required]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string SectionId { get; set; } = string.Empty;
}

public class CancelEnrollmentDto
{
    [Required]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string SectionId { get; set; } = string.Empty;
}

public class EnrollmentResponseDto
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string SectionId { get; set; } = string.Empty;
    public string SubjectId { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string SemesterId { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
