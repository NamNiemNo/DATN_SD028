using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data.Models;
using API.IRepository;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanViensController : ControllerBase
    {
        private readonly INhanVienRepository _repository;

        public NhanViensController(INhanVienRepository repository)
        {
            _repository = repository;
        }

        // GET: api/NhanViens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanViens()
        {
            return Ok(await _repository.GetAllNhanVienAsync());
        }

        // GET: api/NhanViens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(Guid id)
        {
            var nhanVien = await _repository.GetByIdNhanVienAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return Ok(nhanVien);
        }

        // POST: api/NhanViens (Thêm)
        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien([FromBody] NhanVien nhanVien)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.CreateNhanVien(nhanVien);
            return Ok(nhanVien);
        }

        // PUT: api/NhanViens/5 (Sửa)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanVien(Guid id, [FromBody] NhanVien nhanVien)
        {
            if (id != nhanVien.NhanVienId)
            {
                return BadRequest("Id không khớp với dữ liệu nhân viên.");
            }

            try
            {
                await _repository.UpdateNhanVienAsync(nhanVien);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật: {ex.Message}");
            }
        }

        // DELETE: api/NhanViens/5 (Xóa)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(Guid id)
        {
            var nv = await _repository.GetByIdNhanVienAsync(id);
            if (nv == null) return NotFound();

            await _repository.DeleteNhanVienAsync(id);
            return Ok();
        }
    }
}