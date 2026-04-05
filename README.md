# 📚 Course Registration System (Hệ Thống Đăng Ký Tín Chỉ)

Một hệ thống quản lý đăng ký tín chỉ và lớp môn học dành cho sinh viên được thiết kế theo kiến trúc 3-Tier chuẩn mực, sử dụng ASP.NET Core Web API, Entity Framework Core và PostgreSQL. Giao diện frontend được xây dựng bằng HTML, CSS (Vanilla) và JavaScript thuần.

## 🌟 Chức Năng Nổi Bật

### 🎓 Dành cho Sinh Viên
- Hiển thị danh sách các lớp môn học đang mở với trạng thái chi tiết (sĩ số, giảng viên, thời khóa biểu).
- Đăng ký môn học với thao tác nhanh (1 click).
- Hủy lớp tín chỉ đã đăng ký.
- Quản lý "Lớp đã đăng ký" tiện lợi, trực quan (lớp đã đăng ký sẽ được làm mờ trong danh sách).

### ⚙️ Dành cho Quản Trị Viên (Admin / Giảng viên)
- Quản lý thông tin lớp tín chỉ (CRUD).
- Tạo lớp tín chỉ mới với các ràng buộc khóa ngoại được kiểm tra chặt chẽ (Kiểm tra môn học, giảng viên, học kỳ tự động).
- Cập nhật số lượng sinh viên tối đa, thay đổi giảng viên.
- Xóa lớp tín chỉ (tự động xóa lịch học và các đăng ký liên quan một cách an toàn).

## 🛠️ Công Nghệ Sử Dụng

- **Backend:** C# / .NET 10 (ASP.NET Core Web API)
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core (Code-First Approach)
- **Kiến trúc:** 3-Tier Architecture (Controller -> Service -> Database/Repository)
- **Frontend:** HTML5, CSS3, ES6 JavaScript (Vanilla, Fetch API)
- **Tài liệu API:** Swagger (Swashbuckle)

## 📁 Cấu Trúc Dự Án

```text
CourseRegistrationSystem/
├── Controllers/       # Nơi tiếp nhận Request và trả về Response (API Endpoints)
├── Models/            # Các DB Entities cho EF Core (Users, Sections, Enrollments...)
├── DTOs/              # Data Transfer Objects (Request/Response payload data)
├── Services/          # Nơi chứa các Business Logics
├── Data/              # Cấu hình AppDbContext của EF Core
├── Constants/         # Các hằng số (bảng lỗi, etc)
├── wwwroot/           # Frontend Web tĩnh (HTML, CSS, JS)
├── appsettings.json   # Thông tin cấu hình (Chuỗi kết nối DB)
└── Program.cs         # Cấu hình Dependency Injection, Middleware, Host
```

## 🚀 Hướng Dẫn Cài Đặt và Khởi Chạy

### 1. Yêu cầu hệ thống
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) phải được cài đặt trên máy.
- Hệ quản trị cơ sở dữ liệu [PostgreSQL](https://www.postgresql.org/download/). (Có thể dùng PgAdmin4 để quản lý).

### 2. Cấu hình Cơ Sở Dữ Liệu
Mở tệp `appsettings.json` (hoặc `appsettings.Development.json`) và cập nhật cấu hình chuỗi kết nối PostgreSQL của bạn:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=CourseRegistration;Username=postgres;Password=matkhaucuaban"
}
```

### 3. Khởi tạo Database (Migrations)
Mở Terminal / Command Prompt tại thư mục gốc của dự án `CourseRegistrationSystem` và thực thi:
```sh
dotnet ef database update
```
*(Nếu bạn chưa cài tool EF, hãy chạy: `dotnet tool install --global dotnet-ef`)*

### 4. Thêm Data Mẫu (Seed Data)
Để có dữ liệu dùng thử nhanh chóng, bạn có thể thực thi các lệnh SQL (`INSERT INTO`) trực tiếp vào DB của mình hoặc tự viết một lớp SeedData giả lập.

### 5. Chạy Ứng Dụng
```sh
dotnet run
```
- Server Backend sẽ chạy tại cổng `http://localhost:5062` (hoặc cấu hình tương tự ở máy bạn).
- **Trang Front-end:** Truy cập vào `http://localhost:5062` ở trình duyệt.
- **Tài Liệu API:** Truy cập vào `http://localhost:5062/swagger/index.html`.

## 🤝 REST API Endpoints Tiêu Biểu

- `GET /api/sections` - Lấy danh sách toàn bộ lớp tín chỉ
- `POST /api/sections` - Thêm mới lớp tín chỉ
- `POST /api/enrollments/register` - Sinh viên đăng ký lớp
- `POST /api/enrollments/cancel` - Sinh viên hủy đăng ký
- `GET /api/lookup/...` - Api lấy danh sách đổ vào Dropdown

---
*Dự án thực hành chuyên ngành Kỹ thuật phần mềm (KTVTKPM) do @dangkhoiii02 thực hiện.*
