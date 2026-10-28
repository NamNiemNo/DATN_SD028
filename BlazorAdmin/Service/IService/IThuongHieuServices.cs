
using Data.Models;

namespace BlazorAdmin.IServices
{
    public interface IThuongHieuService
    {
        Task<List<ThuongHieu>> GetAllAsync();
        Task<ThuongHieu?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(ThuongHieu item);
        Task<bool> UpdateAsync(Guid id, ThuongHieu item);
        Task<bool> DeleteAsync(Guid id);
    }
}