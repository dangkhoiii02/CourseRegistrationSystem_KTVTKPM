using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.DTOs;

public class CreateSectionDto
{
    [Required]
    public string SectionId { get; set; } = string.Empty;

    [Required]
    public string SubjectId { get; set; } = string.Empty;

    [Required]
    public string SemesterId { get; set; } = string.Empty;

    public int GroupNumber { get; set; }

    [Required]
    public string LecturerId { get; set; } = string.Empty;

    [Range(1, 500)]
    public int MaxCapacity { get; set; }

    public List<CreateScheduleDto> Schedules { get; set; } = new();
}

public class CreateScheduleDto
{
    [Range(2, 8)]
    public int DayOfWeek { get; set; }
    
    [Range(1, 20)]
    public int StartPeriod { get; set; }
    
    [Range(1, 10)]
    public int PeriodCount { get; set; }
    
    public string Room { get; set; } = string.Empty;
}

public class UpdateSectionDto
{
    public string? SubjectId { get; set; }
    public string? LecturerId { get; set; }

    [Range(1, 500)]
    public int? MaxCapacity { get; set; }

    public bool? IsActive { get; set; }
    
    public List<CreateScheduleDto>? Schedules { get; set; }
}

public class SectionResponseDto
{
    public string SectionId { get; set; } = string.Empty;
    public string SubjectId { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string SemesterId { get; set; } = string.Empty;
    public int GroupNumber { get; set; }
    public string LecturerId { get; set; } = string.Empty;
    public string LecturerName { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
    public int RegisteredCount { get; set; }
    public bool IsActive { get; set; }
    public List<ScheduleDto> Schedules { get; set; } = new();
}

public class ScheduleDto
{
    public string ScheduleId { get; set; } = string.Empty;
    public int DayOfWeek { get; set; }
    public int StartPeriod { get; set; }
    public int PeriodCount { get; set; }
    public string Room { get; set; } = string.Empty;
}
