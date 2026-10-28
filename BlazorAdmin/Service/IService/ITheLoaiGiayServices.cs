
using Data.Models;

namespace BlazorAdmin.IServices
{
    public interface ITheLoaiGiayService
    {
        Task<List<TheLoaiGiay>> GetAllAsync();
        Task<TheLoaiGiay?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(TheLoaiGiay item);
        Task<bool> UpdateAsync(Guid id, TheLoaiGiay item);
        Task<bool> DeleteAsync(Guid id);
    }
}