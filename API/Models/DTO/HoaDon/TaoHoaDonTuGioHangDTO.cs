using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.HoaDon
{
    public class TaoHoaDonTuGioHangDTO
    {
        [Required]
        public Guid KhachHangId { get; set; }

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
    }
}
