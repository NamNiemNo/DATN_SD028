using API.IRepository;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Route: api/GioHangChiTiet  (tên class không có chữ "s" để khớp với frontend)
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangChiTietController : ControllerBase
    {
        private readonly IGioHangRepository _repository;

        public GioHangChiTietController(IGioHangRepository repository)
        {
            _repository = repository;
        }

        // POST: api/GioHangChiTiet/AddToCart
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] GioHangChiTiet request)
        {
            var loi = await _repository.AddToCartAsync(request);

            if (loi != null)
                return BadRequest(loi);

            return Ok();
        }

        // DELETE: api/GioHangChiTiet/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var daXoa = await _repository.XoaGioHangChiTietAsync(id);

            if (!daXoa)
                return NotFound("Không tìm thấy sản phẩm trong giỏ hàng.");

            return NoContent();
        }
    }
}