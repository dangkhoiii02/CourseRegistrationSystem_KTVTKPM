using Microsoft.EntityFrameworkCore;
using QuanLyTinChi.Data;
using QuanLyTinChi.DTOs;
using QuanLyTinChi.Models;

namespace QuanLyTinChi.Services
{
    public class SectionService : ISectionService
    {
        private readonly AppDbContext _context;

        public SectionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SectionResponseDto>> GetAllAsync(string? subjectName = null, bool? isActive = null)
        {
            var query = _context.Sections.AsQueryable();

            if (!string.IsNullOrEmpty(subjectName))
                query = query.Where(s => s.SubjectName.Contains(subjectName));

            if (isActive.HasValue)
                query = query.Where(s => s.IsActive == isActive.Value);

            var sections = await query.ToListAsync();

            return sections.Select(MapToDto).ToList();
        }

        public async Task<SectionResponseDto?> GetByIdAsync(string sectionId)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(s => s.SectionId == sectionId);
            if (section == null) return null;

            return MapToDto(section);
        }

        public async Task<SectionResponseDto> CreateAsync(CreateSectionDto dto)
        {
            if (await _context.Sections.AnyAsync(s => s.SectionId == dto.SectionId))
                throw new ArgumentException($"Mã lớp tín chỉ '{dto.SectionId}' đã tồn tại.");

            var section = new Section
            {
                SectionId = dto.SectionId,
                SubjectId = dto.SubjectId,
                SubjectName = dto.SubjectName,
                SemesterName = dto.SemesterName,
                Credits = dto.Credits,
                GroupNumber = dto.GroupNumber,
                LecturerName = dto.LecturerName,
                MaxCapacity = dto.MaxCapacity,
                ScheduleInfo = dto.ScheduleInfo,
                IsActive = dto.IsActive,
                RegisteredCount = 0
            };

            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            return MapToDto(section);
        }

        public async Task<SectionResponseDto?> UpdateAsync(string sectionId, UpdateSectionDto dto)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(s => s.SectionId == sectionId);
            if (section == null) return null;

            if (dto.SubjectId != null) section.SubjectId = dto.SubjectId;
            if (dto.SubjectName != null) section.SubjectName = dto.SubjectName;
            if (dto.SemesterName != null) section.SemesterName = dto.SemesterName;
            if (dto.Credits.HasValue) section.Credits = dto.Credits.Value;
            if (dto.GroupNumber.HasValue) section.GroupNumber = dto.GroupNumber.Value;
            if (dto.LecturerName != null) section.LecturerName = dto.LecturerName;
            if (dto.MaxCapacity.HasValue) section.MaxCapacity = dto.MaxCapacity.Value;
            if (dto.ScheduleInfo != null) section.ScheduleInfo = dto.ScheduleInfo;
            if (dto.IsActive.HasValue) section.IsActive = dto.IsActive.Value;

            await _context.SaveChangesAsync();
            return MapToDto(section);
        }

        public async Task<bool> DeleteAsync(string sectionId)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(s => s.SectionId == sectionId);
            if (section == null) return false;

            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
            return true;
        }

        private static SectionResponseDto MapToDto(Section s)
        {
            return new SectionResponseDto
            {
                SectionId = s.SectionId,
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
                SemesterName = s.SemesterName,
                Credits = s.Credits,
                GroupNumber = s.GroupNumber,
                LecturerName = s.LecturerName,
                MaxCapacity = s.MaxCapacity,
                ScheduleInfo = s.ScheduleInfo,
                RegisteredCount = s.RegisteredCount,
                IsActive = s.IsActive
            };
        }
    }
}
