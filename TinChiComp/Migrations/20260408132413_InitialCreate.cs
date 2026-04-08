using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TinChiComp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    SubjectId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    SubjectName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SemesterName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Credits = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    LecturerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MaxCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleInfo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    RegisteredCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "SectionId", "Credits", "GroupNumber", "IsActive", "LecturerName", "MaxCapacity", "ScheduleInfo", "SemesterName", "SubjectId", "SubjectName" },
                values: new object[,]
                {
                    { "SEC_INT1416_01", 3, 1, true, "Phạm Thị D", 80, "Thứ 5, 5-8, P.104", "Học kỳ 1", "INT1416", "Đảm bảo chất lượng phần mềm" },
                    { "SEC_INT1427_01", 3, 1, true, "Vũ Văn E", 80, "Thứ 6, 1-4, P.105", "Học kỳ 1", "INT1427", "Kiến trúc và thiết kế phần mềm" },
                    { "SEC_INT1448_01", 3, 1, true, "Lê Văn C", 80, "Thứ 4, 1-4, P.103", "Học kỳ 1", "INT1448", "Phát triển phần mềm hướng dịch vụ" },
                    { "SEC_INT1449_03", 3, 3, true, "Trần Thị B", 80, "Thứ 3, 5-8, P.102", "Học kỳ 1", "INT1449", "Phát triển ứng dụng cho các thiết bị di động" },
                    { "SEC_INT1461_01", 3, 1, true, "Nguyễn Văn A", 80, "Thứ 2, 1-4, P.101", "Học kỳ 1", "INT1461", "Xây dựng các hệ thống nhúng" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
