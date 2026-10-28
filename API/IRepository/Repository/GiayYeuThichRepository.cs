
using API.IRepository;
using API.Models;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.IRepository.Repository
{
    public class GiayYeuThichRepository : IGiayYeuThichRepository
    {
        private readonly DbContextApp _context;

        public GiayYeuThichRepository(DbContextApp context)
        {
            _context = context;
        }

        public async Task<List<GiayYeuThich>> GetAllAsync()
        {
            return await _context.giayYeuThiches
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<GiayYeuThich?> GetByIdAsync(Guid id)
        {
            return await _context.giayYeuThiches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<GiayYeuThich> CreateAsync(GiayYeuThich item)
        {
            item.Id = Guid.NewGuid();

            if (item.NgayYeuThich == default)
                item.NgayYeuThich = DateTime.Now;

            await _context.giayYeuThiches.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<bool> UpdateAsync(Guid id, GiayYeuThich item)
        {
            var old = await _context.giayYeuThiches
                .FirstOrDefaultAsync(x => x.Id == id);

            if (old == null)
                return false;

            old.GiayId = item.GiayId;
            old.KhachHangId = item.KhachHangId;
            old.NgayYeuThich = item.NgayYeuThich;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _context.giayYeuThiches
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return false;

            _context.giayYeuThiches.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}