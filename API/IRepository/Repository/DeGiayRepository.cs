
using API.IRepository;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.IRepository.Repository
{
    public class DeGiayRepository : IDeGiayRepository
    {
        private readonly DbContextApp _context;

        public DeGiayRepository(DbContextApp context)
        {
            _context = context;
        }

        public async Task<List<DeGiay>> GetAllAsync()
            => await _context.DeGiays.AsNoTracking().ToListAsync();

        public async Task<DeGiay?> GetByIdAsync(Guid id)
            => await _context.DeGiays.AsNoTracking()
                .FirstOrDefaultAsync(x => x.DeGiayId == id);

        public async Task<DeGiay> CreateAsync(DeGiay item)
        {
            item.DeGiayId = Guid.NewGuid();
            await _context.DeGiays.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateAsync(Guid id, DeGiay item)
        {
            var old = await _context.DeGiays
                .FirstOrDefaultAsync(x => x.DeGiayId == id);

            if (old == null) return false;

            old.TenDeGiay = item.TenDeGiay;
            old.MoTa = item.MoTa;
            old.TrangThai = item.TrangThai;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _context.DeGiays
                .FirstOrDefaultAsync(x => x.DeGiayId == id);

            if (item == null) return false;

            _context.DeGiays.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}