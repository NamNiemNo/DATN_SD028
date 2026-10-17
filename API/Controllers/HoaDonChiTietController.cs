using API.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDonChiTietController : ControllerBase
    {
        private readonly IHoaDonRepository _repository;
        public HoaDonChiTietController(IHoaDonRepository repository) => _repository = repository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repository.GetAllHoaDonChiTietAsync());

        [HttpGet("hoa-don/{hoaDonId:guid}")]
        public async Task<IActionResult> GetByHoaDon(Guid hoaDonId)
            => Ok(await _repository.GetChiTietTheoHoaDonAsync(hoaDonId));
    }
}
