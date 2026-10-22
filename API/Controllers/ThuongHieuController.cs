
using API.IRepository;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuongHieuController : ControllerBase
    {
        private readonly IThuongHieuRepository _repository;

        public ThuongHieuController(IThuongHieuRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repository.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ThuongHieu item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repository.CreateAsync(item);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.ThuongHieuId },
                result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ThuongHieu item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _repository.UpdateAsync(id, item))
                return NotFound();

            return Ok(new { message = "Cập nhật thương hiệu thành công." });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _repository.DeleteAsync(id))
                return NotFound();

            return Ok(new { message = "Xóa thương hiệu thành công." });
        }
    }
}
