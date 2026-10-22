
using API.IRepository;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheLoaiGiayController : ControllerBase
    {
        private readonly ITheLoaiGiayRepository _repository;

        public TheLoaiGiayController(ITheLoaiGiayRepository repository)
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
        public async Task<IActionResult> Create(TheLoaiGiay item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _repository.CreateAsync(item);
            return CreatedAtAction(nameof(GetById),
                new { id = result.TheLoaiGiayId }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, TheLoaiGiay item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _repository.UpdateAsync(id, item)) return NotFound();

            return Ok(new { message = "Cập nhật thể loại giày thành công." });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _repository.DeleteAsync(id)) return NotFound();
            return Ok(new { message = "Xóa thể loại giày thành công." });
        }
    }
}
