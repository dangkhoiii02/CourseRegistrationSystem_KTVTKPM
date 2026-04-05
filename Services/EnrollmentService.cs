using Microsoft.EntityFrameworkCore;
using CourseRegistrationSystem.Data;
using CourseRegistrationSystem.DTOs;
using CourseRegistrationSystem.Models;

namespace CourseRegistrationSystem.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly AppDbContext _context;

    public EnrollmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string Message, EnrollmentResponseDto? Data)> RegisterAsync(RegisterEnrollmentDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Kiểm tra 1: Lớp tín chỉ có tồn tại và đang hoạt động không?
            var section = await _context.Sections
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(s => s.SectionId == dto.SectionId);

            if (section == null)
                return (false, "Lớp tín chỉ không tồn tại.", null);

            if (!section.IsActive)
                return (false, "Lớp tín chỉ đã bị khóa, không thể đăng ký.", null);

            // Kiểm tra sinh viên tồn tại
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId);

            if (student == null)
                return (false, "Sinh viên không tồn tại.", null);

            // Kiểm tra 2: Lớp đã đầy chưa?
            if (section.RegisteredCount >= section.MaxCapacity)
                return (false, $"Lớp đã đầy ({section.RegisteredCount}/{section.MaxCapacity}).", null);

            // Kiểm tra 3: Sinh viên đã đăng ký lớp này chưa?
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == dto.StudentId
                            && e.SectionId == dto.SectionId);

            if (existingEnrollment != null)
            {
                if (existingEnrollment.Status == EnrollmentStatus.Active)
                    return (false, "Sinh viên đã đăng ký lớp tín chỉ này rồi.", null);

                // Reactivate cancelled enrollment
                existingEnrollment.Status = EnrollmentStatus.Active;
                existingEnrollment.EnrolledAt = DateTime.UtcNow;

                section.RegisteredCount += 1;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Đăng ký thành công!", new EnrollmentResponseDto
                {
                    Id = existingEnrollment.Id,
                    StudentId = existingEnrollment.StudentId,
                    StudentName = student.User.FullName,
                    SectionId = existingEnrollment.SectionId,
                    SubjectId = section.SubjectId,
                    SubjectName = section.Subject.SubjectName,
                    SemesterId = section.SemesterId,
                    EnrolledAt = existingEnrollment.EnrolledAt,
                    Status = existingEnrollment.Status.ToString()
                });
            }

            // Tạo mới Enrollment
            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                SectionId = dto.SectionId,
                EnrolledAt = DateTime.UtcNow,
                Status = EnrollmentStatus.Active
            };

            _context.Enrollments.Add(enrollment);

            // Cộng RegisteredCount
            section.RegisteredCount += 1;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var responseDto = new EnrollmentResponseDto
            {
                Id = enrollment.Id,
                StudentId = enrollment.StudentId,
                StudentName = student.User.FullName,
                SectionId = enrollment.SectionId,
                SubjectId = section.SubjectId,
                SubjectName = section.Subject.SubjectName,
                SemesterId = section.SemesterId,
                EnrolledAt = enrollment.EnrolledAt,
                Status = enrollment.Status.ToString()
            };

            return (true, "Đăng ký thành công!", responseDto);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<(bool Success, string Message)> CancelAsync(CancelEnrollmentDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == dto.StudentId
                                       && e.SectionId == dto.SectionId
                                       && e.Status == EnrollmentStatus.Active);

            if (enrollment == null)
                return (false, "Không tìm thấy đăng ký hoạt động cho sinh viên này trong lớp tín chỉ đã chọn.");

            enrollment.Status = EnrollmentStatus.Cancelled;

            var section = await _context.Sections.FindAsync(dto.SectionId);
            if (section != null)
            {
                section.RegisteredCount = Math.Max(0, section.RegisteredCount - 1);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (true, "Hủy đăng ký thành công!");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<EnrollmentResponseDto>> GetByStudentAsync(string studentId)
    {
        return await _context.Enrollments
            .Include(e => e.Section).ThenInclude(s => s.Subject)
            .Include(e => e.Student).ThenInclude(s => s.User)
            .Where(e => e.StudentId == studentId && e.Status == EnrollmentStatus.Active)
            .OrderByDescending(e => e.EnrolledAt)
            .Select(e => new EnrollmentResponseDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                StudentName = e.Student.User.FullName,
                SectionId = e.SectionId,
                SubjectId = e.Section.SubjectId,
                SubjectName = e.Section.Subject.SubjectName,
                SemesterId = e.Section.SemesterId,
                EnrolledAt = e.EnrolledAt,
                Status = e.Status.ToString()
            })
            .ToListAsync();
    }
}
