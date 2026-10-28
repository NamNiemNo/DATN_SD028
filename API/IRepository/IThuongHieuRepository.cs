
using Data.Models;

namespace API.IRepository
{
    public interface IThuongHieuRepository
    {
        Task<List<ThuongHieu>> GetAllAsync();
        Task<ThuongHieu?> GetByIdAsync(Guid id);
        Task<ThuongHieu> CreateAsync(ThuongHieu thuongHieu);
        Task<bool> UpdateAsync(Guid id, ThuongHieu thuongHieu);
        Task<bool> DeleteAsync(Guid id);
    }
}