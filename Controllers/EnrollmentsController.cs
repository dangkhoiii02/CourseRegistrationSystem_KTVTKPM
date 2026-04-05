using Microsoft.AspNetCore.Mvc;
using CourseRegistrationSystem.DTOs;
using CourseRegistrationSystem.Services;

namespace CourseRegistrationSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterEnrollmentDto dto)
    {
        try
        {
            var (success, message, data) = await _enrollmentService.RegisterAsync(dto);
            if (!success)
                return BadRequest(new { message });
            return CreatedAtAction(nameof(GetByStudent), new { studentId = dto.StudentId }, new { message, data });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Lỗi đăng ký: {ex.InnerException?.Message ?? ex.Message}" });
        }
    }

    [HttpPost("cancel")]
    public async Task<ActionResult> Cancel([FromBody] CancelEnrollmentDto dto)
    {
        try
        {
            var (success, message) = await _enrollmentService.CancelAsync(dto);
            if (!success)
                return BadRequest(new { message });
            return Ok(new { message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Lỗi hủy: {ex.InnerException?.Message ?? ex.Message}" });
        }
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<List<EnrollmentResponseDto>>> GetByStudent(string studentId)
    {
        var enrollments = await _enrollmentService.GetByStudentAsync(studentId);
        return Ok(enrollments);
    }
}
