using CourseRegistrationSystem.DTOs;

namespace CourseRegistrationSystem.Services;

public interface ISectionService
{
    Task<List<SectionResponseDto>> GetAllAsync(string? semester = null);
    Task<SectionResponseDto?> GetByIdAsync(string id);
    Task<SectionResponseDto> CreateAsync(CreateSectionDto dto);
    Task<SectionResponseDto?> UpdateAsync(string id, UpdateSectionDto dto);
    Task<(bool Success, string Message)> DeleteAsync(string id);
}
