using System.ComponentModel.DataAnnotations;

namespace QuanLyTinChi.DTOs
{
    public class SectionResponseDto
    {
        public string SectionId { get; set; } = string.Empty;
        public string SubjectId { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string SemesterName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int GroupNumber { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public string ScheduleInfo { get; set; } = string.Empty;
        public int RegisteredCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateSectionDto
    {
        [Required]
        [MaxLength(30)]
        public string SectionId { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string SubjectId { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string SubjectName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SemesterName { get; set; } = string.Empty;

        [Required]
        [Range(1, 10)]
        public int Credits { get; set; }

        [Required]
        [Range(1, 100)]
        public int GroupNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string LecturerName { get; set; } = string.Empty;

        [Required]
        [Range(10, 200)]
        public int MaxCapacity { get; set; }

        [Required]
        [MaxLength(200)]
        public string ScheduleInfo { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateSectionDto
    {
        [MaxLength(20)]
        public string? SubjectId { get; set; }

        [MaxLength(200)]
        public string? SubjectName { get; set; }

        [MaxLength(100)]
        public string? SemesterName { get; set; }

        [Range(1, 10)]
        public int? Credits { get; set; }

        [Range(1, 100)]
        public int? GroupNumber { get; set; }

        [MaxLength(100)]
        public string? LecturerName { get; set; }

        [Range(10, 200)]
        public int? MaxCapacity { get; set; }

        [MaxLength(200)]
        public string? ScheduleInfo { get; set; }

        public bool? IsActive { get; set; }
    }
}
