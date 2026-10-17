using API.IRepository;
using API.Models.DTO.HoaDon;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepository _repository;
        public HoaDonController(IHoaDonRepository repository) => _repository = repository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repository.GetAllHoaDonAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var hoaDon = await _repository.GetHoaDonByIdAsync(id);
            return hoaDon == null ? NotFound(new { message = "Không tìm thấy hóa đơn." }) : Ok(hoaDon);
        }

        [HttpGet("khach-hang/{khachHangId:guid}")]
        public async Task<IActionResult> GetByKhachHang(Guid khachHangId)
            => Ok(await _repository.GetHoaDonTheoKhachHangAsync(khachHangId));

        [HttpPost("tao-tu-gio-hang")]
        public async Task<IActionResult> TaoTuGioHang([FromBody] TaoHoaDonTuGioHangDTO request)
        {
            var (hoaDon, loi) = await _repository.TaoHoaDonTuGioHangAsync(request);
            if (loi != null) return BadRequest(new { message = loi });
            return CreatedAtAction(nameof(GetById), new { id = hoaDon!.HoaDonId }, hoaDon);
        }

        [HttpPost("tao-thu-cong")]
        public async Task<IActionResult> TaoThuCong([FromBody] TaoHoaDonThuCongDTO request)
        {
            var (hoaDon, loi) = await _repository.TaoHoaDonThuCongAsync(request);
            if (loi != null) return BadRequest(new { message = loi });
            return CreatedAtAction(nameof(GetById), new { id = hoaDon!.HoaDonId }, hoaDon);
        }
    }
}
