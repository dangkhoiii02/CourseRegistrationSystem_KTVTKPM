using Microsoft.EntityFrameworkCore;
using CourseRegistrationSystem.Data;
using CourseRegistrationSystem.DTOs;
using CourseRegistrationSystem.Models;

namespace CourseRegistrationSystem.Services;

public class SectionService : ISectionService
{
    private readonly AppDbContext _context;

    public SectionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SectionResponseDto>> GetAllAsync(string? semester = null)
    {
        var query = _context.Sections
            .Include(s => s.Subject)
            .Include(s => s.Lecturer).ThenInclude(l => l.User)
            .Include(s => s.Schedules)
            .AsQueryable();

        if (!string.IsNullOrEmpty(semester))
            query = query.Where(s => s.SemesterId == semester);

        return await query
            .Where(s => s.IsActive)
            .OrderBy(s => s.SubjectId)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    public async Task<SectionResponseDto?> GetByIdAsync(string id)
    {
        var section = await _context.Sections
            .Include(s => s.Subject)
            .Include(s => s.Lecturer).ThenInclude(l => l.User)
            .Include(s => s.Schedules)
            .FirstOrDefaultAsync(s => s.SectionId == id);

        return section == null ? null : MapToDto(section);
    }

    public async Task<SectionResponseDto> CreateAsync(CreateSectionDto dto)
    {
        // Validate FK references
        if (!await _context.Subjects.AnyAsync(s => s.SubjectId == dto.SubjectId))
            throw new ArgumentException($"Mã môn học '{dto.SubjectId}' không tồn tại.");

        if (!await _context.Semesters.AnyAsync(s => s.SemesterId == dto.SemesterId))
            throw new ArgumentException($"Mã học kỳ '{dto.SemesterId}' không tồn tại.");

        if (!await _context.Lecturers.AnyAsync(l => l.LecturerId == dto.LecturerId))
            throw new ArgumentException($"Mã giảng viên '{dto.LecturerId}' không tồn tại.");

        if (await _context.Sections.AnyAsync(s => s.SectionId == dto.SectionId))
            throw new ArgumentException($"Mã lớp '{dto.SectionId}' đã tồn tại.");

        var section = new Section
        {
            SectionId = dto.SectionId,
            SubjectId = dto.SubjectId,
            SemesterId = dto.SemesterId,
            GroupNumber = dto.GroupNumber,
            LecturerId = dto.LecturerId,
            MaxCapacity = dto.MaxCapacity,
            RegisteredCount = 0,
            IsActive = true
        };

        _context.Sections.Add(section);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync(section.SectionId))!;
    }

    public async Task<SectionResponseDto?> UpdateAsync(string id, UpdateSectionDto dto)
    {
        var section = await _context.Sections.FindAsync(id);
        if (section == null) return null;

        if (dto.LecturerId != null) section.LecturerId = dto.LecturerId;
        if (dto.MaxCapacity.HasValue) section.MaxCapacity = dto.MaxCapacity.Value;
        if (dto.IsActive.HasValue) section.IsActive = dto.IsActive.Value;

        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<(bool Success, string Message)> DeleteAsync(string id)
    {
        var section = await _context.Sections
            .Include(s => s.Schedules)
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.SectionId == id);

        if (section == null)
            return (false, "Không tìm thấy lớp tín chỉ.");

        if (section.Enrollments.Any(e => e.Status == EnrollmentStatus.Active))
        {
            section.IsActive = false;
            await _context.SaveChangesAsync();
            return (true, "Lớp đã có sinh viên đăng ký nên được chuyển sang trạng thái không hoạt động.");
        }

        // Xóa schedules và enrollments (cancelled) liên quan trước
        _context.Schedules.RemoveRange(section.Schedules);
        _context.Enrollments.RemoveRange(section.Enrollments);
        _context.Sections.Remove(section);
        await _context.SaveChangesAsync();
        return (true, "Đã xóa lớp tín chỉ thành công.");
    }

    private static SectionResponseDto MapToDto(Section s) => new()
    {
        SectionId = s.SectionId,
        SubjectId = s.SubjectId,
        SubjectName = s.Subject?.SubjectName ?? "",
        Credits = s.Subject?.Credits ?? 0,
        SemesterId = s.SemesterId,
        GroupNumber = s.GroupNumber,
        LecturerId = s.LecturerId,
        LecturerName = s.Lecturer?.User?.FullName ?? "",
        MaxCapacity = s.MaxCapacity,
        RegisteredCount = s.RegisteredCount,
        IsActive = s.IsActive,
        Schedules = s.Schedules?.Select(sch => new ScheduleDto
        {
            ScheduleId = sch.ScheduleId,
            DayOfWeek = sch.DayOfWeek,
            StartPeriod = sch.StartPeriod,
            PeriodCount = sch.PeriodCount,
            Room = sch.Room
        }).ToList() ?? new()
    };
}
