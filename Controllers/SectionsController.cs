using Microsoft.AspNetCore.Mvc;
using CourseRegistrationSystem.DTOs;
using CourseRegistrationSystem.Services;

namespace CourseRegistrationSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionsController : ControllerBase
{
    private readonly ISectionService _sectionService;

    public SectionsController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SectionResponseDto>>> GetAll([FromQuery] string? semester = null)
    {
        var sections = await _sectionService.GetAllAsync(semester);
        return Ok(sections);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SectionResponseDto>> GetById(string id)
    {
        var section = await _sectionService.GetByIdAsync(id);
        if (section == null)
            return NotFound(new { message = "Không tìm thấy lớp tín chỉ." });
        return Ok(section);
    }

    [HttpPost]
    public async Task<ActionResult<SectionResponseDto>> Create([FromBody] CreateSectionDto dto)
    {
        try
        {
            var section = await _sectionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = section.SectionId }, section);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Lỗi khi thêm lớp: {ex.InnerException?.Message ?? ex.Message}" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SectionResponseDto>> Update(string id, [FromBody] UpdateSectionDto dto)
    {
        try
        {
            var section = await _sectionService.UpdateAsync(id, dto);
            if (section == null)
                return NotFound(new { message = "Không tìm thấy lớp tín chỉ." });
            return Ok(section);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Lỗi khi cập nhật: {ex.InnerException?.Message ?? ex.Message}" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        try
        {
            var (success, message) = await _sectionService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message });
            return Ok(new { message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Lỗi khi xóa: {ex.InnerException?.Message ?? ex.Message}" });
        }
    }
}
