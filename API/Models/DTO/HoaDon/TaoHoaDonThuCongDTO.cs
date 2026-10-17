using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.HoaDon
{
    public class TaoHoaDonThuCongDTO
    {
        public Guid? KhachHangId { get; set; }
        public Guid? NhanVienId { get; set; }

        [Required, MaxLength(100)]
        public string TenNguoiNhan { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required, MaxLength(300)]
        public string DiaChi { get; set; } = string.Empty;

        public Guid? HinhThucThanhToanId { get; set; }
        public Guid? VoucherId { get; set; }
        public float PhiShip { get; set; }
        [MaxLength(200)]
        public string? GhiChu { get; set; }

        [Required, MinLength(1)]
        public List<TaoHoaDonChiTietDTO> ChiTiet { get; set; } = new();
    }

    public class TaoHoaDonChiTietDTO
    {
        [Required]
        public Guid GiayChiTietId { get; set; }

        [Range(1, int.MaxValue)]
        public int SoLuongSanPham { get; set; }
    }
}
