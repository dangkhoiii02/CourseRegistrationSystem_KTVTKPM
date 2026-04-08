using QuanLyTinChi.DTOs;

namespace QuanLyTinChi.Services
{
    public interface ISectionService
    {
        Task<List<SectionResponseDto>> GetAllAsync(string? subjectName = null, bool? isActive = null);
        Task<SectionResponseDto?> GetByIdAsync(string sectionId);
        Task<SectionResponseDto> CreateAsync(CreateSectionDto dto);
        Task<SectionResponseDto?> UpdateAsync(string sectionId, UpdateSectionDto dto);
        Task<bool> DeleteAsync(string sectionId);
    }
}
