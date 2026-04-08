using System.ComponentModel.DataAnnotations;

namespace QuanLyTinChi.Models
{
    public class Section
    {
        [Key]
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

        public int Credits { get; set; }

        public int GroupNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string LecturerName { get; set; } = string.Empty;

        public int MaxCapacity { get; set; }

        [Required]
        [MaxLength(200)]
        public string ScheduleInfo { get; set; } = string.Empty;

        public int RegisteredCount { get; set; }

        public bool IsActive { get; set; }
    }
}
