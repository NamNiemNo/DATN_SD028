
using Data.Models;

namespace BlazorAdmin.IServices
{
    public interface IDeGiayService
    {
        Task<List<DeGiay>> GetAllAsync();
        Task<DeGiay?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(DeGiay item);
        Task<bool> UpdateAsync(Guid id, DeGiay item);
        Task<bool> DeleteAsync(Guid id);
    }
}