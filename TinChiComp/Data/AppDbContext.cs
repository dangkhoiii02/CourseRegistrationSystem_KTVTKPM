using Microsoft.EntityFrameworkCore;
using QuanLyTinChi.Models;

namespace QuanLyTinChi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Section> Sections => Set<Section>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Section ----
            modelBuilder.Entity<Section>(e =>
            {
                e.HasKey(s => s.SectionId);
                e.Property(s => s.RegisteredCount).HasDefaultValue(0);
                e.Property(s => s.IsActive).HasDefaultValue(true);
            });

            // ===== SEED DATA =====
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Tạm thời chèn data mẫu
            modelBuilder.Entity<Section>().HasData(
                new Section { SectionId = "SEC_INT1461_01", SubjectId = "INT1461", SubjectName = "Xây dựng các hệ thống nhúng", SemesterName = "Học kỳ 1", LecturerName = "Nguyễn Văn A", ScheduleInfo = "Thứ 2, 1-4, P.101", Credits = 3, GroupNumber = 1, MaxCapacity = 80, RegisteredCount = 0, IsActive = true },
                new Section { SectionId = "SEC_INT1449_03", SubjectId = "INT1449", SubjectName = "Phát triển ứng dụng cho các thiết bị di động", SemesterName = "Học kỳ 1", LecturerName = "Trần Thị B", ScheduleInfo = "Thứ 3, 5-8, P.102", Credits = 3, GroupNumber = 3, MaxCapacity = 80, RegisteredCount = 0, IsActive = true },
                new Section { SectionId = "SEC_INT1448_01", SubjectId = "INT1448", SubjectName = "Phát triển phần mềm hướng dịch vụ", SemesterName = "Học kỳ 1", LecturerName = "Lê Văn C", ScheduleInfo = "Thứ 4, 1-4, P.103", Credits = 3, GroupNumber = 1, MaxCapacity = 80, RegisteredCount = 0, IsActive = true },
                new Section { SectionId = "SEC_INT1416_01", SubjectId = "INT1416", SubjectName = "Đảm bảo chất lượng phần mềm", SemesterName = "Học kỳ 1", LecturerName = "Phạm Thị D", ScheduleInfo = "Thứ 5, 5-8, P.104", Credits = 3, GroupNumber = 1, MaxCapacity = 80, RegisteredCount = 0, IsActive = true },
                new Section { SectionId = "SEC_INT1427_01", SubjectId = "INT1427", SubjectName = "Kiến trúc và thiết kế phần mềm", SemesterName = "Học kỳ 1", LecturerName = "Vũ Văn E", ScheduleInfo = "Thứ 6, 1-4, P.105", Credits = 3, GroupNumber = 1, MaxCapacity = 80, RegisteredCount = 0, IsActive = true }
            );
        }
    }
}
