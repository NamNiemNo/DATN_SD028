using API.IRepository;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Route: api/GioHangs
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangsController : ControllerBase
    {
        private readonly IGioHangRepository _repository;

        public GioHangsController(IGioHangRepository repository)
        {
            _repository = repository;
        }

        // GET: api/GioHangs/GetAllGioHang
        [HttpGet("GetAllGioHang")]
        public async Task<ActionResult<List<GioHang>>> GetAllGioHang()
        {
            return Ok(await _repository.GetAllGioHangAsync());
        }

        // POST: api/GioHangs/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] GioHang gioHang)
        {
            var (ketQua, loi) = await _repository.CreateGioHangAsync(gioHang);

            if (loi != null)
                return BadRequest(loi);

            return Ok(ketQua);
        }

        // GET: api/GioHangs/khach-hang/{id}
        // id = TaiKhoanId (hoặc KhachHangId)
        [HttpGet("khach-hang/{id:guid}")]
        public async Task<IActionResult> GetTheoKhachHang(Guid id)
        {
            return Ok(await _repository.GetGioHangDTOTheoKhachHangAsync(id));
        }

        // PUT: api/GioHangs/cap-nhat-so-luong?id=...&soLuong=...
        [HttpPut("cap-nhat-so-luong")]
        public async Task<IActionResult> CapNhatSoLuong([FromQuery] Guid id, [FromQuery] int soLuong)
        {
            var loi = await _repository.CapNhatSoLuongAsync(id, soLuong);

            if (loi != null)
                return BadRequest(loi);

            return Ok();
        }
    }
}