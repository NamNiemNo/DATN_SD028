using API.Models.DTO;
using Data.Models;

namespace API.IRepository
{
    public interface IGioHangRepository
    {
        // ===== GIỎ HÀNG =====
        Task<List<GioHang>> GetAllGioHangAsync();

        // Trả về giỏ hàng, hoặc thông báo lỗi nếu không tạo được.
        Task<(GioHang? GioHang, string? Loi)> CreateGioHangAsync(GioHang gioHang);

        // id có thể là TaiKhoanId (frontend đang gọi theo tài khoản) hoặc KhachHangId.
        Task<GioHangDTO> GetGioHangDTOTheoKhachHangAsync(Guid id);

        // Trả về null nếu thành công, ngược lại trả về thông báo lỗi.
        Task<string?> CapNhatSoLuongAsync(Guid gioHangChiTietId, int soLuong);

        // ===== GIỎ HÀNG CHI TIẾT =====
        // Trả về null nếu thành công, ngược lại trả về thông báo lỗi.
        Task<string?> AddToCartAsync(GioHangChiTiet request);

        Task<bool> XoaGioHangChiTietAsync(Guid gioHangChiTietId);
    }
}