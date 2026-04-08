using Microsoft.AspNetCore.Mvc;
using QuanLyTinChi.DTOs;
using QuanLyTinChi.Services;

namespace QuanLyTinChi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TinChiController : ControllerBase
    {
        private readonly QuanLyTinChi.TinChiComp _tinChiComp;
        private readonly ISectionService _sectionService;

        public TinChiController(QuanLyTinChi.TinChiComp tinChiComp, ISectionService sectionService)
        {
            _tinChiComp = tinChiComp;
            _sectionService = sectionService;
        }

        [HttpGet("kiemtra")]
        public ActionResult<bool> KiemTraHanMuc([FromQuery] int hienTai, [FromQuery] int them)
        {
            return Ok(_tinChiComp.KiemTraHanMuc(hienTai, them));
        }

        [HttpPost("dangky")]
        public ActionResult<string> DangKyHocPhan([FromQuery] string maSV, [FromQuery] string maMon)
        {
            var result = _tinChiComp.DangKyHocPhan(maSV, maMon);
            return Ok(new { message = result });
        }

        [HttpGet("hocphi")]
        public ActionResult<double> TinhHocPhi([FromQuery] int soTinChi)
        {
            return Ok(_tinChiComp.TinhHocPhi(soTinChi));
        }

        // ===== CÁC API DÀNH CHO SECTIONS CHUYỂN TỪ SECTIONSCONTROLLER SANG =====

        [HttpGet("/api/sections")]
        public async Task<ActionResult<List<SectionResponseDto>>> GetSections([FromQuery] string? subjectName, [FromQuery] bool? isActive)
        {
            var sections = await _sectionService.GetAllAsync(subjectName, isActive);
            return Ok(sections);
        }

        [HttpGet("/api/sections/{id}")]
        public async Task<ActionResult<SectionResponseDto>> GetSection(string id)
        {
            var section = await _sectionService.GetByIdAsync(id);
            if (section == null) return NotFound(new { message = $"Không tìm thấy lớp tín chỉ mã {id}" });

            return Ok(section);
        }

        [HttpPost("/api/sections")]
        public async Task<ActionResult<SectionResponseDto>> CreateSection([FromBody] CreateSectionDto dto)
        {
            try
            {
                var section = await _sectionService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetSection), new { id = section.SectionId }, section);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("/api/sections/{id}")]
        public async Task<ActionResult<SectionResponseDto>> UpdateSection(string id, [FromBody] UpdateSectionDto dto)
        {
            var section = await _sectionService.UpdateAsync(id, dto);
            if (section == null) return NotFound(new { message = $"Không tìm thấy lớp tín chỉ mã {id}" });

            return Ok(section);
        }

        [HttpDelete("/api/sections/{id}")]
        public async Task<ActionResult> DeleteSection(string id)
        {
            var success = await _sectionService.DeleteAsync(id);
            if (!success) return NotFound(new { message = $"Không tìm thấy lớp tín chỉ mã {id}" });

            return Ok(new { message = "Xóa lớp tín chỉ thành công" });
        }
    }
}
