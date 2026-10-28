
using Data.Models;

namespace API.IRepository
{
    public interface ITheLoaiGiayRepository
    {
        Task<List<TheLoaiGiay>> GetAllAsync();
        Task<TheLoaiGiay?> GetByIdAsync(Guid id);
        Task<TheLoaiGiay> CreateAsync(TheLoaiGiay item);
        Task<bool> UpdateAsync(Guid id, TheLoaiGiay item);
        Task<bool> DeleteAsync(Guid id);
    }
}