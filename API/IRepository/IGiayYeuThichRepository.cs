
using API.Models;

namespace API.IRepository
{
    public interface IGiayYeuThichRepository
    {
        Task<List<GiayYeuThich>> GetAllAsync();
        Task<GiayYeuThich?> GetByIdAsync(Guid id);
        Task<GiayYeuThich> CreateAsync(GiayYeuThich item);
        Task<bool> UpdateAsync(Guid id, GiayYeuThich item);
        Task<bool> DeleteAsync(Guid id);
    }
}
