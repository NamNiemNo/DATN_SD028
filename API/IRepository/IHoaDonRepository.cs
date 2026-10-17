using API.Models.DTO.HoaDon;
using Data.Models;

namespace API.IRepository
{
    public interface IHoaDonRepository
    {
        Task<List<HoaDon>> GetAllHoaDonAsync();
        Task<HoaDon?> GetHoaDonByIdAsync(Guid hoaDonId);
        Task<List<HoaDon>> GetHoaDonTheoKhachHangAsync(Guid khachHangId);
        Task<List<HoaDonChiTiet>> GetAllHoaDonChiTietAsync();
        Task<List<HoaDonChiTiet>> GetChiTietTheoHoaDonAsync(Guid hoaDonId);
        Task<(HoaDon? HoaDon, string? Loi)> TaoHoaDonTuGioHangAsync(TaoHoaDonTuGioHangDTO request);
        Task<(HoaDon? HoaDon, string? Loi)> TaoHoaDonThuCongAsync(TaoHoaDonThuCongDTO request);
    }
}
