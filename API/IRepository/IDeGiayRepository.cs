
using Data.Models;

namespace API.IRepository
{
    public interface IDeGiayRepository
    {
        Task<List<DeGiay>> GetAllAsync();
        Task<DeGiay?> GetByIdAsync(Guid id);
        Task<DeGiay> CreateAsync(DeGiay item);
        Task<bool> UpdateAsync(Guid id, DeGiay item);
        Task<bool> DeleteAsync(Guid id);
    }
}