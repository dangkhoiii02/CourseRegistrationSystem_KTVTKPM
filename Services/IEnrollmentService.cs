using CourseRegistrationSystem.DTOs;

namespace CourseRegistrationSystem.Services;

public interface IEnrollmentService
{
    Task<(bool Success, string Message, EnrollmentResponseDto? Data)> RegisterAsync(RegisterEnrollmentDto dto);
    Task<(bool Success, string Message)> CancelAsync(CancelEnrollmentDto dto);
    Task<List<EnrollmentResponseDto>> GetByStudentAsync(string studentId);
}
