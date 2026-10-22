
using API.Models;

namespace BlazorAdmin.IServices
{
    public interface IGiayYeuThichService
    {
        Task<List<GiayYeuThich>> GetAllAsync();
        Task<GiayYeuThich?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(GiayYeuThich item);
        Task<bool> UpdateAsync(Guid id, GiayYeuThich item);
        Task<bool> DeleteAsync(Guid id);
    }
}
