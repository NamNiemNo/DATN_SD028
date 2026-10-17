using API.IRepository;
using API.Models.DTO.HoaDon;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.IRepository.Repository
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly DbContextApp _db;

        public HoaDonRepository(DbContextApp db) => _db = db;

        public async Task<List<HoaDon>> GetAllHoaDonAsync()
        {
            return await QueryHoaDon().OrderByDescending(x => x.NgayTao).ToListAsync();
        }

        public async Task<HoaDon?> GetHoaDonByIdAsync(Guid hoaDonId)
        {
            return await QueryHoaDon().FirstOrDefaultAsync(x => x.HoaDonId == hoaDonId);
        }

        public async Task<List<HoaDon>> GetHoaDonTheoKhachHangAsync(Guid khachHangId)
        {
            return await QueryHoaDon()
                .Where(x => x.KhachHangId == khachHangId)
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync();
        }

        public async Task<List<HoaDonChiTiet>> GetAllHoaDonChiTietAsync()
        {
            return await QueryHoaDonChiTiet().ToListAsync();
        }

        public async Task<List<HoaDonChiTiet>> GetChiTietTheoHoaDonAsync(Guid hoaDonId)
        {
            return await QueryHoaDonChiTiet().Where(x => x.HoaDonId == hoaDonId).ToListAsync();
        }

        public async Task<(HoaDon? HoaDon, string? Loi)> TaoHoaDonTuGioHangAsync(TaoHoaDonTuGioHangDTO request)
        {
            if (request.PhiShip < 0) return (null, "Phí ship không được âm.");
            if (string.IsNullOrWhiteSpace(request.TenNguoiNhan) ||
                string.IsNullOrWhiteSpace(request.SoDienThoai) ||
                string.IsNullOrWhiteSpace(request.DiaChi))
                return (null, "Vui lòng nhập đầy đủ thông tin người nhận.");

            var khachHang = await _db.KhachHangs.FindAsync(request.KhachHangId);
            if (khachHang == null) return (null, "Không tìm thấy khách hàng.");

            var gioHang = await _db.GioHangs
                .Include(g => g.GioHangChiTiets)
                .FirstOrDefaultAsync(g => g.KhachHangId == request.KhachHangId);
            if (gioHang == null || !gioHang.GioHangChiTiets.Any())
                return (null, "Giỏ hàng của khách hàng đang trống.");

            var lines = gioHang.GioHangChiTiets
                .GroupBy(x => x.GiayChiTietId)
                .Select(g => new TaoHoaDonChiTietDTO
                {
                    GiayChiTietId = g.Key,
                    SoLuongSanPham = g.Sum(x => x.SoLuongSanPham)
                }).ToList();

            return await CreateInvoiceAsync(
                request.KhachHangId, null, request.TenNguoiNhan, request.SoDienThoai,
                request.DiaChi, request.HinhThucThanhToanId, request.VoucherId,
                request.PhiShip, request.GhiChu, lines, gioHang);
        }

        public async Task<(HoaDon? HoaDon, string? Loi)> TaoHoaDonThuCongAsync(TaoHoaDonThuCongDTO request)
        {
            if (request.PhiShip < 0) return (null, "Phí ship không được âm.");
            if (request.ChiTiet == null || request.ChiTiet.Count == 0)
                return (null, "Hóa đơn phải có ít nhất một sản phẩm.");
            if (request.ChiTiet.Any(x => x.GiayChiTietId == Guid.Empty || x.SoLuongSanPham <= 0))
                return (null, "Sản phẩm hoặc số lượng không hợp lệ.");

            if (request.KhachHangId.HasValue &&
                !await _db.KhachHangs.AnyAsync(x => x.KhachHangId == request.KhachHangId.Value))
                return (null, "Không tìm thấy khách hàng.");
            if (request.NhanVienId.HasValue &&
                !await _db.NhanViens.AnyAsync(x => x.NhanVienId == request.NhanVienId.Value))
                return (null, "Không tìm thấy nhân viên.");

            var lines = request.ChiTiet
                .GroupBy(x => x.GiayChiTietId)
                .Select(g => new TaoHoaDonChiTietDTO
                {
                    GiayChiTietId = g.Key,
                    SoLuongSanPham = g.Sum(x => x.SoLuongSanPham)
                }).ToList();

            return await CreateInvoiceAsync(
                request.KhachHangId, request.NhanVienId, request.TenNguoiNhan,
                request.SoDienThoai, request.DiaChi, request.HinhThucThanhToanId,
                request.VoucherId, request.PhiShip, request.GhiChu, lines, null);
        }

        private async Task<(HoaDon? HoaDon, string? Loi)> CreateInvoiceAsync(
            Guid? khachHangId, Guid? nhanVienId, string tenNguoiNhan,
            string soDienThoai, string diaChi, Guid? hinhThucThanhToanId,
            Guid? voucherId, float phiShip, string? ghiChu,
            List<TaoHoaDonChiTietDTO> requestedLines, GioHang? gioHang)
        {
            if (requestedLines.Count == 0) return (null, "Hóa đơn phải có ít nhất một sản phẩm.");
            if (string.IsNullOrWhiteSpace(tenNguoiNhan) || string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(diaChi))
                return (null, "Vui lòng nhập đầy đủ thông tin người nhận.");

            if (hinhThucThanhToanId.HasValue &&
                !await _db.hinhThucThanhToans.AnyAsync(x => x.HinhThucThanhToanId == hinhThucThanhToanId.Value))
                return (null, "Hình thức thanh toán không tồn tại.");
            if (voucherId.HasValue && !await _db.Vouchers.AnyAsync(x => x.VoucherId == voucherId.Value))
                return (null, "Voucher không tồn tại.");

            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var ids = requestedLines.Select(x => x.GiayChiTietId).Distinct().ToList();
                var variants = await _db.GiayChiTiets
                    .Include(x => x.Giay)
                    .Where(x => ids.Contains(x.GiayChiTietId))
                    .ToListAsync();

                if (variants.Count != ids.Count)
                    return (null, "Có sản phẩm chi tiết không tồn tại.");

                // Kiểm tra toàn bộ sản phẩm trước khi thay đổi tồn kho.
                foreach (var line in requestedLines)
                {
                    var variant = variants.First(x => x.GiayChiTietId == line.GiayChiTietId);
                    if (line.SoLuongSanPham <= 0) return (null, "Số lượng sản phẩm phải lớn hơn 0.");
                    if (!variant.TrangThai || variant.Giay == null || !variant.Giay.TrangThai)
                        return (null, $"Sản phẩm {line.GiayChiTietId} hiện không kinh doanh.");
                    if (variant.SoLuongCon < line.SoLuongSanPham)
                        return (null, $"Sản phẩm '{variant.Giay.TenGiay}' không đủ tồn kho. Còn {variant.SoLuongCon} sản phẩm.");
                    if (variant.Gia < 0) return (null, "Giá sản phẩm không hợp lệ.");
                }

                decimal subtotal = 0;
                var invoice = new HoaDon
                {
                    HoaDonId = Guid.NewGuid(),
                    KhachHangId = khachHangId,
                    NhanVienId = nhanVienId,
                    HinhThucThanhToanId = hinhThucThanhToanId,
                    VoucherId = voucherId,
                    TenNguoiNhan = tenNguoiNhan.Trim(),
                    SoDienThoai = soDienThoai.Trim(),
                    DiaChi = diaChi.Trim(),
                    NgayTao = DateTime.Now,
                    NgayNhanHang = DateTime.Now.AddDays(3),
                    PhiShip = phiShip,
                    TrangThai = "Chờ xác nhận",
                    TrangThaiThanhToan = "Chưa thanh toán",
                    GhiChu = ghiChu?.Trim() ?? string.Empty
                };

                foreach (var line in requestedLines)
                {
                    var variant = variants.First(x => x.GiayChiTietId == line.GiayChiTietId);
                    var donGia = Convert.ToDecimal(variant.Gia);
                    var thanhTien = donGia * line.SoLuongSanPham;
                    subtotal += thanhTien;

                    invoice.HoaDonChiTiets.Add(new HoaDonChiTiet
                    {
                        HoaDonChiTietId = Guid.NewGuid(),
                        HoaDonId = invoice.HoaDonId,
                        GiayChiTietId = variant.GiayChiTietId,
                        SoLuongSanPham = line.SoLuongSanPham,
                        DonGia = donGia,
                        Gia = thanhTien,
                        TrangThai = false,
                        TrangThaiChiTiet = "Đang mua"
                    });

                    // Chỉ trừ tồn kho khi hóa đơn được tạo thành công trong transaction.
                    variant.SoLuongCon -= line.SoLuongSanPham;
                    variant.SoLuongDat += line.SoLuongSanPham;
                    variant.NgaySua = DateTime.Now;
                }

                // Voucher chưa được tự động tính giảm vì model hiện tại chưa cung cấp quy tắc áp dụng ở đây.
                invoice.TongTienSauKhiGiam = (float)(subtotal + Convert.ToDecimal(phiShip));
                await _db.HoaDons.AddAsync(invoice);

                if (gioHang != null)
                {
                    _db.GioHangChiTiets.RemoveRange(gioHang.GioHangChiTiets);
                    gioHang.NgayCapNhatCuoiCung = DateTime.Now;
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                var created = await GetHoaDonByIdAsync(invoice.HoaDonId);
                return (created, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (null, $"Không thể tạo hóa đơn: {ex.Message}");
            }
        }

        private IQueryable<HoaDonChiTiet> QueryHoaDonChiTiet()
        {
            return _db.HoaDonChiTiets.AsNoTracking()
                .Include(x => x.GiayChiTiet).ThenInclude(x => x!.Giay)
                .Include(x => x.GiayChiTiet).ThenInclude(x => x!.KichCo)
                .Include(x => x.GiayChiTiet).ThenInclude(x => x!.MauSac);
        }

        private IQueryable<HoaDon> QueryHoaDon()
        {
            return _db.HoaDons
                .AsNoTracking()
                .Include(x => x.khachHang)
                .Include(x => x.nhanVien)
                .Include(x => x.hinhThucThanhToan)
                .Include(x => x.voucher)
                .Include(x => x.HoaDonChiTiets)
                    .ThenInclude(ct => ct.GiayChiTiet)
                        .ThenInclude(gc => gc.Giay)
                .Include(x => x.HoaDonChiTiets)
                    .ThenInclude(ct => ct.GiayChiTiet)
                        .ThenInclude(gc => gc.KichCo)
                .Include(x => x.HoaDonChiTiets)
                    .ThenInclude(ct => ct.GiayChiTiet)
                        .ThenInclude(gc => gc.MauSac);
        }
    }
}
