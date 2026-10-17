using API.Models.DTO;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.IRepository.Repository
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly DbContextApp _db;

        public GioHangRepository(DbContextApp db)
        {
            _db = db;
        }

        // =====================================================
        // GIỎ HÀNG
        // =====================================================

        public async Task<List<GioHang>> GetAllGioHangAsync()
        {
            return await _db.GioHangs
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(GioHang? GioHang, string? Loi)> CreateGioHangAsync(GioHang gioHang)
        {
            var khachHangTonTai = await _db.KhachHangs
                .AnyAsync(k => k.KhachHangId == gioHang.KhachHangId);

            if (!khachHangTonTai)
                return (null, "Không tìm thấy khách hàng.");

            // Mỗi khách hàng chỉ có 1 giỏ đang dùng: nếu có rồi thì trả về giỏ đó.
            var gioHangCu = await _db.GioHangs
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.KhachHangId == gioHang.KhachHangId && g.TrangThai);

            if (gioHangCu != null)
                return (gioHangCu, null);

            if (gioHang.GioHangId == Guid.Empty)
                gioHang.GioHangId = Guid.NewGuid();

            gioHang.NgayTaoGioHang = DateTime.Now;
            gioHang.NgayCapNhatCuoiCung = DateTime.Now;
            gioHang.TrangThai = true;

            _db.GioHangs.Add(gioHang);
            await _db.SaveChangesAsync();

            return (gioHang, null);
        }

        public async Task<GioHangDTO> GetGioHangDTOTheoKhachHangAsync(Guid id)
        {
            // Frontend gọi theo TaiKhoanId, nên đổi sang KhachHangId trước.
            // Nếu không tìm thấy khách hàng theo tài khoản thì coi id là KhachHangId.
            var khachHangId = await _db.KhachHangs
                .Where(k => k.TaiKhoanId == id)
                .Select(k => (Guid?)k.KhachHangId)
                .FirstOrDefaultAsync() ?? id;

            var danhSach = await _db.GioHangChiTiets
                .AsNoTracking()
                .Where(c => c.GioHang != null
                            && c.GioHang.KhachHangId == khachHangId
                            && c.GioHang.TrangThai)
                .OrderBy(c => c.NgayTao)
                .Select(c => new GioHangChiTietDTO
                {
                    GioHangChiTietId = c.GioHangChiTietId,
                    GiayChiTietId = c.GiayChiTietId,
                    TenGiay = c.Giays != null ? c.Giays.TenGiay : "",
                    Size = c.GiayChiTiet != null && c.GiayChiTiet.KichCo != null
                        ? c.GiayChiTiet.KichCo.TenKichCo
                        : null,
                    MauSac = c.GiayChiTiet != null && c.GiayChiTiet.MauSac != null
                        ? c.GiayChiTiet.MauSac.TenMau
                        : null,
                    HinhAnh = c.GiayChiTiet != null
                        ? c.GiayChiTiet.Anhs
                            .Where(a => a.TrangThai)
                            .Select(a => a.DuongDan)
                            .FirstOrDefault()
                        : null,
                    SoLuong = c.SoLuongSanPham,
                    Gia = c.Gia
                })
                .ToListAsync();

            return new GioHangDTO { DanhSachChiTiet = danhSach };
        }

        public async Task<string?> CapNhatSoLuongAsync(Guid gioHangChiTietId, int soLuong)
        {
            if (soLuong < 1)
                return "Số lượng phải từ 1 trở lên.";

            var chiTiet = await _db.GioHangChiTiets
                .Include(c => c.GiayChiTiet)
                .FirstOrDefaultAsync(c => c.GioHangChiTietId == gioHangChiTietId);

            if (chiTiet == null)
                return "Không tìm thấy sản phẩm trong giỏ hàng.";

            if (chiTiet.GiayChiTiet != null && soLuong > chiTiet.GiayChiTiet.SoLuongCon)
                return $"Chỉ còn {chiTiet.GiayChiTiet.SoLuongCon} sản phẩm trong kho.";

            chiTiet.SoLuongSanPham = soLuong;
            chiTiet.NgayCapNhat = DateTime.Now;

            await _db.SaveChangesAsync();
            return null;
        }

        // =====================================================
        // GIỎ HÀNG CHI TIẾT
        // =====================================================

        public async Task<string?> AddToCartAsync(GioHangChiTiet request)
        {
            if (request.SoLuongSanPham < 1)
                return "Số lượng phải từ 1 trở lên.";

            var gioHang = await _db.GioHangs
                .FirstOrDefaultAsync(g => g.GioHangId == request.GioHangId);

            if (gioHang == null)
                return "Không tìm thấy giỏ hàng.";

            var giayChiTiet = await _db.GiayChiTiets
                .FirstOrDefaultAsync(g => g.GiayChiTietId == request.GiayChiTietId);

            if (giayChiTiet == null)
                return "Không tìm thấy sản phẩm.";

            // Sản phẩm này đã có trong giỏ chưa?
            var daCo = await _db.GioHangChiTiets
                .FirstOrDefaultAsync(c => c.GioHangId == request.GioHangId
                                          && c.GiayChiTietId == request.GiayChiTietId);

            var tongSoLuong = request.SoLuongSanPham + (daCo?.SoLuongSanPham ?? 0);

            if (tongSoLuong > giayChiTiet.SoLuongCon)
                return $"Chỉ còn {giayChiTiet.SoLuongCon} sản phẩm trong kho.";

            // Luôn lấy giá từ database, không tin giá do client gửi lên.
            var gia = (decimal)giayChiTiet.Gia;
            var now = DateTime.Now;

            if (daCo != null)
            {
                daCo.SoLuongSanPham = tongSoLuong;
                daCo.Gia = gia;
                daCo.NgayCapNhat = now;
            }
            else
            {
                _db.GioHangChiTiets.Add(new GioHangChiTiet
                {
                    GioHangChiTietId = Guid.NewGuid(),
                    GioHangId = request.GioHangId,
                    GiayChiTietId = request.GiayChiTietId,
                    GiayId = giayChiTiet.GiayId,
                    SoLuongSanPham = request.SoLuongSanPham,
                    Gia = gia,
                    TrangThai = true,
                    NgayTao = now,
                    NgayCapNhat = now
                });
            }

            gioHang.NgayCapNhatCuoiCung = now;

            await _db.SaveChangesAsync();
            return null;
        }

        public async Task<bool> XoaGioHangChiTietAsync(Guid gioHangChiTietId)
        {
            var chiTiet = await _db.GioHangChiTiets.FindAsync(gioHangChiTietId);

            if (chiTiet == null)
                return false;

            _db.GioHangChiTiets.Remove(chiTiet);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}