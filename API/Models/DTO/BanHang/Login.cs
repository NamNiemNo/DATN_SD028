namespace API.Models.DTO.BanHang
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? TaiKhoanId { get; set; }
        public string? Username { get; set; }
        public string? TenNhanVien { get; set; }
    }
}
