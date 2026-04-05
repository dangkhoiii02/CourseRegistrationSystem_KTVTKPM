using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseRegistrationSystem.Data;

namespace CourseRegistrationSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupController : ControllerBase
{
    private readonly AppDbContext _context;

    public LookupController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("subjects")]
    public async Task<ActionResult> GetSubjects()
    {
        var subjects = await _context.Subjects
            .OrderBy(s => s.SubjectId)
            .Select(s => new { s.SubjectId, s.SubjectName, s.Credits })
            .ToListAsync();
        return Ok(subjects);
    }

    [HttpGet("lecturers")]
    public async Task<ActionResult> GetLecturers()
    {
        var lecturers = await _context.Lecturers
            .Include(l => l.User)
            .OrderBy(l => l.LecturerId)
            .Select(l => new { l.LecturerId, FullName = l.User.FullName, l.Degree })
            .ToListAsync();
        return Ok(lecturers);
    }

    [HttpGet("semesters")]
    public async Task<ActionResult> GetSemesters()
    {
        var semesters = await _context.Semesters
            .OrderByDescending(s => s.SemesterId)
            .Select(s => new { s.SemesterId, s.SemesterName })
            .ToListAsync();
        return Ok(semesters);
    }
}
