using Microsoft.EntityFrameworkCore;
using CourseRegistrationSystem.Models;

namespace CourseRegistrationSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Faculty> Faculties => Set<Faculty>();
    public DbSet<StudentClass> Classes => Set<StudentClass>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Lecturer> Lecturers => Set<Lecturer>();
    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<RegistrationPeriod> RegistrationPeriods => Set<RegistrationPeriod>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- Role ----
        modelBuilder.Entity<Role>(e =>
        {
            e.HasKey(r => r.RoleId);
        });

        // ---- User ----
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
            e.Property(u => u.Gender).HasConversion<int>();
            e.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            e.HasIndex(u => u.Username).IsUnique();
        });

        // ---- Faculty ----
        modelBuilder.Entity<Faculty>(e =>
        {
            e.HasKey(f => f.FacultyId);
        });

        // ---- StudentClass (Table: Classes) ----
        modelBuilder.Entity<StudentClass>(e =>
        {
            e.HasKey(c => c.ClassId);
            e.HasOne(c => c.Faculty).WithMany(f => f.Classes).HasForeignKey(c => c.FacultyId);
        });

        // ---- Student ----
        modelBuilder.Entity<Student>(e =>
        {
            e.HasKey(s => s.StudentId);
            e.HasOne(s => s.User).WithOne(u => u.Student).HasForeignKey<Student>(s => s.UserId);
            e.HasOne(s => s.Class).WithMany(c => c.Students).HasForeignKey(s => s.ClassId);
        });

        // ---- Lecturer ----
        modelBuilder.Entity<Lecturer>(e =>
        {
            e.HasKey(l => l.LecturerId);
            e.HasOne(l => l.User).WithOne(u => u.Lecturer).HasForeignKey<Lecturer>(l => l.UserId);
            e.HasOne(l => l.Faculty).WithMany(f => f.Lecturers).HasForeignKey(l => l.FacultyId);
        });

        // ---- Semester ----
        modelBuilder.Entity<Semester>(e =>
        {
            e.HasKey(s => s.SemesterId);
        });

        // ---- Subject ----
        modelBuilder.Entity<Subject>(e =>
        {
            e.HasKey(s => s.SubjectId);
            e.HasOne(s => s.Faculty).WithMany(f => f.Subjects).HasForeignKey(s => s.FacultyId);
        });

        // ---- Section ----
        modelBuilder.Entity<Section>(e =>
        {
            e.HasKey(s => s.SectionId);
            e.Property(s => s.RegisteredCount).HasDefaultValue(0);
            e.Property(s => s.IsActive).HasDefaultValue(true);
            e.HasOne(s => s.Subject).WithMany(sub => sub.Sections).HasForeignKey(s => s.SubjectId);
            e.HasOne(s => s.Semester).WithMany(sem => sem.Sections).HasForeignKey(s => s.SemesterId);
            e.HasOne(s => s.Lecturer).WithMany(l => l.Sections).HasForeignKey(s => s.LecturerId);
        });

        // ---- Schedule ----
        modelBuilder.Entity<Schedule>(e =>
        {
            e.HasKey(s => s.ScheduleId);
            e.HasOne(s => s.Section).WithMany(sec => sec.Schedules).HasForeignKey(s => s.SectionId);
        });

        // ---- RegistrationPeriod ----
        modelBuilder.Entity<RegistrationPeriod>(e =>
        {
            e.HasKey(r => r.PeriodId);
            e.HasOne(r => r.Semester).WithMany(s => s.RegistrationPeriods).HasForeignKey(r => r.SemesterId);
        });

        // ---- Enrollment ----
        modelBuilder.Entity<Enrollment>(e =>
        {
            e.HasKey(en => en.Id);
            e.Property(en => en.Status).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(en => new { en.StudentId, en.SectionId }).IsUnique();
            e.HasIndex(en => en.StudentId);
            e.HasIndex(en => en.SectionId);
            e.HasOne(en => en.Student).WithMany(s => s.Enrollments).HasForeignKey(en => en.StudentId);
            e.HasOne(en => en.Section).WithMany(sec => sec.Enrollments).HasForeignKey(en => en.SectionId).OnDelete(DeleteBehavior.Restrict);
        });

        // ===== SEED DATA =====
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = "R_STUDENT", RoleName = "Student" },
            new Role { RoleId = "R_LECTURER", RoleName = "Lecturer" },
            new Role { RoleId = "R_ADMIN", RoleName = "Admin" }
        );

        // Users
        modelBuilder.Entity<User>().HasData(
            new User { UserId = "U_KHOI", Username = "khoipd_ptit", Password = "hashed_password", RoleId = "R_STUDENT", IsActive = true, FullName = "Phạm Đăng Khôi", NationalId = "079123456789", DateOfBirth = new DateOnly(2004, 5, 15), Gender = Gender.Male, PhoneNumber = "0901234567", Email = "khoipd@student.ptithcm.edu.vn", Address = "97 Man Thiện, P. Hiệp Phú, TP. Thủ Đức, TP.HCM" },
            new User { UserId = "U_KIEN", Username = "kiennt", Password = "hashed_password", RoleId = "R_LECTURER", IsActive = true, FullName = "Nguyễn Trọng Kiên", NationalId = "079222222221", DateOfBirth = new DateOnly(1985, 1, 1), Gender = Gender.Male, PhoneNumber = "0902222221", Email = "kiennt@ptithcm.edu.vn", Address = "Q. Bình Thạnh, TP.HCM" },
            new User { UserId = "U_HIEU", Username = "hieunt", Password = "hashed_password", RoleId = "R_LECTURER", IsActive = true, FullName = "Nguyễn Trung Hiếu", NationalId = "079333333332", DateOfBirth = new DateOnly(1988, 2, 2), Gender = Gender.Male, PhoneNumber = "0903333332", Email = "hieunt@ptithcm.edu.vn", Address = "Q. 9, TP.HCM" },
            new User { UserId = "U_THE", Username = "thept", Password = "hashed_password", RoleId = "R_LECTURER", IsActive = true, FullName = "Phan Thị Thể", NationalId = "079444444443", DateOfBirth = new DateOnly(1987, 3, 3), Gender = Gender.Female, PhoneNumber = "0904444443", Email = "thept@ptithcm.edu.vn", Address = "Q. Gò Vấp, TP.HCM" },
            new User { UserId = "U_HAO", Username = "haona", Password = "hashed_password", RoleId = "R_LECTURER", IsActive = true, FullName = "Nguyễn Anh Hào", NationalId = "079555555554", DateOfBirth = new DateOnly(1990, 4, 4), Gender = Gender.Male, PhoneNumber = "0905555554", Email = "haona@ptithcm.edu.vn", Address = "Q. Thủ Đức, TP.HCM" },
            new User { UserId = "U_HOANG", Username = "hoangnvh", Password = "hashed_password", RoleId = "R_LECTURER", IsActive = true, FullName = "Nguyễn Văn Hữu Hoàng", NationalId = "079666666665", DateOfBirth = new DateOnly(1982, 5, 5), Gender = Gender.Male, PhoneNumber = "0906666665", Email = "hoangnvh@ptithcm.edu.vn", Address = "Q. 10, TP.HCM" }
        );

        // Faculty
        modelBuilder.Entity<Faculty>().HasData(
            new Faculty { FacultyId = "F_IT", FacultyName = "Khoa Công nghệ Thông tin 2" }
        );

        // Classes
        modelBuilder.Entity<StudentClass>().HasData(
            new StudentClass { ClassId = "D22CQCN01-N", ClassName = "D22CQCN01-N", FacultyId = "F_IT", AcademicYear = "2022-2026", EducationType = "Đại học chính quy" }
        );

        // Students
        modelBuilder.Entity<Student>().HasData(
            new Student { StudentId = "1", UserId = "U_KHOI", ClassId = "D22CQCN01-N" }
        );

        // Lecturers
        modelBuilder.Entity<Lecturer>().HasData(
            new Lecturer { LecturerId = "L_KIEN", UserId = "U_KIEN", FacultyId = "F_IT", Degree = "Thạc sĩ" },
            new Lecturer { LecturerId = "L_HIEU", UserId = "U_HIEU", FacultyId = "F_IT", Degree = "Tiến sĩ" },
            new Lecturer { LecturerId = "L_THE", UserId = "U_THE", FacultyId = "F_IT", Degree = "Thạc sĩ" },
            new Lecturer { LecturerId = "L_HAO", UserId = "U_HAO", FacultyId = "F_IT", Degree = "Thạc sĩ" },
            new Lecturer { LecturerId = "L_HOANG", UserId = "U_HOANG", FacultyId = "F_IT", Degree = "Tiến sĩ" }
        );

        // Semesters
        modelBuilder.Entity<Semester>().HasData(
            new Semester { SemesterId = "HK2_2324", SemesterName = "Học kỳ 2", AcademicYear = "2023-2024" }
        );

        // Subjects
        modelBuilder.Entity<Subject>().HasData(
            new Subject { SubjectId = "INT1461", SubjectName = "Xây dựng các hệ thống nhúng", Credits = 3, FacultyId = "F_IT" },
            new Subject { SubjectId = "INT1449", SubjectName = "Phát triển ứng dụng cho các thiết bị di động", Credits = 3, FacultyId = "F_IT" },
            new Subject { SubjectId = "INT1448", SubjectName = "Phát triển phần mềm hướng dịch vụ", Credits = 3, FacultyId = "F_IT" },
            new Subject { SubjectId = "INT1416", SubjectName = "Đảm bảo chất lượng phần mềm", Credits = 3, FacultyId = "F_IT" },
            new Subject { SubjectId = "INT1427", SubjectName = "Kiến trúc và thiết kế phần mềm", Credits = 3, FacultyId = "F_IT" }
        );

        // Sections
        modelBuilder.Entity<Section>().HasData(
            new Section { SectionId = "SEC_INT1461_01", SubjectId = "INT1461", SemesterId = "HK2_2324", GroupNumber = 1, LecturerId = "L_KIEN", MaxCapacity = 80, RegisteredCount = 0, IsActive = true },
            new Section { SectionId = "SEC_INT1449_03", SubjectId = "INT1449", SemesterId = "HK2_2324", GroupNumber = 3, LecturerId = "L_HIEU", MaxCapacity = 80, RegisteredCount = 0, IsActive = true },
            new Section { SectionId = "SEC_INT1448_01", SubjectId = "INT1448", SemesterId = "HK2_2324", GroupNumber = 1, LecturerId = "L_THE", MaxCapacity = 80, RegisteredCount = 0, IsActive = true },
            new Section { SectionId = "SEC_INT1416_01", SubjectId = "INT1416", SemesterId = "HK2_2324", GroupNumber = 1, LecturerId = "L_HAO", MaxCapacity = 80, RegisteredCount = 0, IsActive = true },
            new Section { SectionId = "SEC_INT1427_01", SubjectId = "INT1427", SemesterId = "HK2_2324", GroupNumber = 1, LecturerId = "L_HOANG", MaxCapacity = 80, RegisteredCount = 0, IsActive = true }
        );

        // Schedules
        modelBuilder.Entity<Schedule>().HasData(
            new Schedule { ScheduleId = "SCH_01", SectionId = "SEC_INT1461_01", DayOfWeek = 2, StartPeriod = 1, PeriodCount = 4, Room = "2A33" },
            new Schedule { ScheduleId = "SCH_02", SectionId = "SEC_INT1449_03", DayOfWeek = 2, StartPeriod = 5, PeriodCount = 4, Room = "2A27" },
            new Schedule { ScheduleId = "SCH_03", SectionId = "SEC_INT1448_01", DayOfWeek = 4, StartPeriod = 1, PeriodCount = 4, Room = "HTD" },
            new Schedule { ScheduleId = "SCH_04", SectionId = "SEC_INT1416_01", DayOfWeek = 4, StartPeriod = 5, PeriodCount = 4, Room = "2A33" },
            new Schedule { ScheduleId = "SCH_05", SectionId = "SEC_INT1427_01", DayOfWeek = 4, StartPeriod = 9, PeriodCount = 4, Room = "HTD" }
        );

        // Registration Periods
        modelBuilder.Entity<RegistrationPeriod>().HasData(
            new RegistrationPeriod { PeriodId = "REG_HK2_2324", PeriodName = "Đăng ký tín chỉ HK2 - Khóa 2022", SemesterId = "HK2_2324", StartTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2028, 12, 31, 23, 59, 59, DateTimeKind.Utc), TargetAudience = "D22CQCN" }
        );
    }
}
