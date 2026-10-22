
using API.IRepository;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.IRepository.Repository
{
    public class TheLoaiGiayRepository : ITheLoaiGiayRepository
    {
        private readonly DbContextApp _context;

        public TheLoaiGiayRepository(DbContextApp context)
        {
            _context = context;
        }

        public async Task<List<TheLoaiGiay>> GetAllAsync()
            => await _context.TheLoaiGiays.AsNoTracking().ToListAsync();

        public async Task<TheLoaiGiay?> GetByIdAsync(Guid id)
            => await _context.TheLoaiGiays.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TheLoaiGiayId == id);

        public async Task<TheLoaiGiay> CreateAsync(TheLoaiGiay item)
        {
            item.TheLoaiGiayId = Guid.NewGuid();
            await _context.TheLoaiGiays.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateAsync(Guid id, TheLoaiGiay item)
        {
            var old = await _context.TheLoaiGiays
                .FirstOrDefaultAsync(x => x.TheLoaiGiayId == id);

            if (old == null) return false;

            old.TenTheLoai = item.TenTheLoai;
            old.MoTa = item.MoTa;
            old.TrangThai = item.TrangThai;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _context.TheLoaiGiays
                .FirstOrDefaultAsync(x => x.TheLoaiGiayId == id);

            if (item == null) return false;

            _context.TheLoaiGiays.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
