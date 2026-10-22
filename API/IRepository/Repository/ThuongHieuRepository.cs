
using API.IRepository;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.IRepository.Repository
{
    public class ThuongHieuRepository : IThuongHieuRepository
    {
        private readonly DbContextApp _context;

        public ThuongHieuRepository(DbContextApp context)
        {
            _context = context;
        }

        public async Task<List<ThuongHieu>> GetAllAsync()
        {
            return await _context.ThuongHieus
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ThuongHieu?> GetByIdAsync(Guid id)
        {
            return await _context.ThuongHieus
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ThuongHieuId == id);
        }

        public async Task<ThuongHieu> CreateAsync(ThuongHieu thuongHieu)
        {
            thuongHieu.ThuongHieuId = Guid.NewGuid();

            await _context.ThuongHieus.AddAsync(thuongHieu);
            await _context.SaveChangesAsync();

            return thuongHieu;
        }

        public async Task<bool> UpdateAsync(Guid id, ThuongHieu thuongHieu)
        {
            var item = await _context.ThuongHieus
                .FirstOrDefaultAsync(x => x.ThuongHieuId == id);

            if (item == null)
                return false;

            item.TenThuongHieu = thuongHieu.TenThuongHieu;
            item.Email = thuongHieu.Email;
            item.SDT = thuongHieu.SDT;
            item.DiaChi = thuongHieu.DiaChi;
            item.MoTa = thuongHieu.MoTa;
            item.TrangThai = thuongHieu.TrangThai;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _context.ThuongHieus
                .FirstOrDefaultAsync(x => x.ThuongHieuId == id);

            if (item == null)
                return false;

            _context.ThuongHieus.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
