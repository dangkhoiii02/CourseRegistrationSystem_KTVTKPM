using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourseRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FacultyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RoleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    SemesterId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SemesterName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AcademicYear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.SemesterId);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ClassName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FacultyId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AcademicYear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EducationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_Classes_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SubjectName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Credits = table.Column<int>(type: "integer", nullable: false),
                    FacultyId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                    table.ForeignKey(
                        name: "FK_Subjects_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RoleId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NationalId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationPeriods",
                columns: table => new
                {
                    PeriodId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PeriodName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SemesterId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TargetAudience = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationPeriods", x => x.PeriodId);
                    table.ForeignKey(
                        name: "FK_RegistrationPeriods_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    LecturerId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FacultyId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Degree = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.LecturerId);
                    table.ForeignKey(
                        name: "FK_Lecturers_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lecturers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClassId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SubjectId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SemesterId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GroupNumber = table.Column<int>(type: "integer", nullable: false),
                    LecturerId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MaxCapacity = table.Column<int>(type: "integer", nullable: false),
                    RegisteredCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
                    table.ForeignKey(
                        name: "FK_Sections_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SectionId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SectionId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartPeriod = table.Column<int>(type: "integer", nullable: false),
                    PeriodCount = table.Column<int>(type: "integer", nullable: false),
                    Room = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedules_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "FacultyId", "FacultyName" },
                values: new object[] { "F_IT", "Khoa Công nghệ Thông tin 2" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { "R_ADMIN", "Admin" },
                    { "R_LECTURER", "Lecturer" },
                    { "R_STUDENT", "Student" }
                });

            migrationBuilder.InsertData(
                table: "Semesters",
                columns: new[] { "SemesterId", "AcademicYear", "SemesterName" },
                values: new object[] { "HK2_2324", "2023-2024", "Học kỳ 2" });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassId", "AcademicYear", "ClassName", "EducationType", "FacultyId" },
                values: new object[] { "D22CQCN01-N", "2022-2026", "D22CQCN01-N", "Đại học chính quy", "F_IT" });

            migrationBuilder.InsertData(
                table: "RegistrationPeriods",
                columns: new[] { "PeriodId", "EndTime", "PeriodName", "SemesterId", "StartTime", "TargetAudience" },
                values: new object[] { "REG_HK2_2324", new DateTime(2028, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "Đăng ký tín chỉ HK2 - Khóa 2022", "HK2_2324", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "D22CQCN" });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "SubjectId", "Credits", "FacultyId", "SubjectName" },
                values: new object[,]
                {
                    { "INT1416", 3, "F_IT", "Đảm bảo chất lượng phần mềm" },
                    { "INT1427", 3, "F_IT", "Kiến trúc và thiết kế phần mềm" },
                    { "INT1448", 3, "F_IT", "Phát triển phần mềm hướng dịch vụ" },
                    { "INT1449", 3, "F_IT", "Phát triển ứng dụng cho các thiết bị di động" },
                    { "INT1461", 3, "F_IT", "Xây dựng các hệ thống nhúng" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "DateOfBirth", "Email", "FullName", "Gender", "IsActive", "NationalId", "Password", "PhoneNumber", "RoleId", "Username" },
                values: new object[,]
                {
                    { "U_HAO", "Q. Thủ Đức, TP.HCM", new DateOnly(1990, 4, 4), "haona@ptithcm.edu.vn", "Nguyễn Anh Hào", 0, true, "079555555554", "hashed_password", "0905555554", "R_LECTURER", "haona" },
                    { "U_HIEU", "Q. 9, TP.HCM", new DateOnly(1988, 2, 2), "hieunt@ptithcm.edu.vn", "Nguyễn Trung Hiếu", 0, true, "079333333332", "hashed_password", "0903333332", "R_LECTURER", "hieunt" },
                    { "U_HOANG", "Q. 10, TP.HCM", new DateOnly(1982, 5, 5), "hoangnvh@ptithcm.edu.vn", "Nguyễn Văn Hữu Hoàng", 0, true, "079666666665", "hashed_password", "0906666665", "R_LECTURER", "hoangnvh" },
                    { "U_KHOI", "97 Man Thiện, P. Hiệp Phú, TP. Thủ Đức, TP.HCM", new DateOnly(2004, 5, 15), "khoipd@student.ptithcm.edu.vn", "Phạm Đăng Khôi", 0, true, "079123456789", "hashed_password", "0901234567", "R_STUDENT", "khoipd_ptit" },
                    { "U_KIEN", "Q. Bình Thạnh, TP.HCM", new DateOnly(1985, 1, 1), "kiennt@ptithcm.edu.vn", "Nguyễn Trọng Kiên", 0, true, "079222222221", "hashed_password", "0902222221", "R_LECTURER", "kiennt" },
                    { "U_THE", "Q. Gò Vấp, TP.HCM", new DateOnly(1987, 3, 3), "thept@ptithcm.edu.vn", "Phan Thị Thể", 1, true, "079444444443", "hashed_password", "0904444443", "R_LECTURER", "thept" }
                });

            migrationBuilder.InsertData(
                table: "Lecturers",
                columns: new[] { "LecturerId", "Degree", "FacultyId", "UserId" },
                values: new object[,]
                {
                    { "L_HAO", "Thạc sĩ", "F_IT", "U_HAO" },
                    { "L_HIEU", "Tiến sĩ", "F_IT", "U_HIEU" },
                    { "L_HOANG", "Tiến sĩ", "F_IT", "U_HOANG" },
                    { "L_KIEN", "Thạc sĩ", "F_IT", "U_KIEN" },
                    { "L_THE", "Thạc sĩ", "F_IT", "U_THE" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "ClassId", "UserId" },
                values: new object[] { "1", "D22CQCN01-N", "U_KHOI" });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "SectionId", "GroupNumber", "IsActive", "LecturerId", "MaxCapacity", "SemesterId", "SubjectId" },
                values: new object[,]
                {
                    { "SEC_INT1416_01", 1, true, "L_HAO", 80, "HK2_2324", "INT1416" },
                    { "SEC_INT1427_01", 1, true, "L_HOANG", 80, "HK2_2324", "INT1427" },
                    { "SEC_INT1448_01", 1, true, "L_THE", 80, "HK2_2324", "INT1448" },
                    { "SEC_INT1449_03", 3, true, "L_HIEU", 80, "HK2_2324", "INT1449" },
                    { "SEC_INT1461_01", 1, true, "L_KIEN", 80, "HK2_2324", "INT1461" }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ScheduleId", "DayOfWeek", "PeriodCount", "Room", "SectionId", "StartPeriod" },
                values: new object[,]
                {
                    { "SCH_01", 2, 4, "2A33", "SEC_INT1461_01", 1 },
                    { "SCH_02", 2, 4, "2A27", "SEC_INT1449_03", 5 },
                    { "SCH_03", 4, 4, "HTD", "SEC_INT1448_01", 1 },
                    { "SCH_04", 4, 4, "2A33", "SEC_INT1416_01", 5 },
                    { "SCH_05", 4, 4, "HTD", "SEC_INT1427_01", 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_FacultyId",
                table: "Classes",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SectionId",
                table: "Enrollments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_SectionId",
                table: "Enrollments",
                columns: new[] { "StudentId", "SectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_FacultyId",
                table: "Lecturers",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_UserId",
                table: "Lecturers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationPeriods_SemesterId",
                table: "RegistrationPeriods",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SectionId",
                table: "Schedules",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_LecturerId",
                table: "Sections",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SemesterId",
                table: "Sections",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SubjectId",
                table: "Sections",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_FacultyId",
                table: "Subjects",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "RegistrationPeriods");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Lecturers");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
