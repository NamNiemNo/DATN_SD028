using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TraHangs_GiayChiTiets_GiayChiTietId",
                table: "TraHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_TraHangs_HoaDons_HoaDonId",
                table: "TraHangs");

            migrationBuilder.DropTable(
                name: "TinNhanAIs");

            migrationBuilder.DropTable(
                name: "CuocTroChuyenAIs");

            migrationBuilder.DeleteData(
                table: "hinhThucThanhToans",
                keyColumn: "HinhThucThanhToanId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "hinhThucThanhToans",
                keyColumn: "HinhThucThanhToanId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.AddForeignKey(
                name: "FK_TraHangs_GiayChiTiets_GiayChiTietId",
                table: "TraHangs",
                column: "GiayChiTietId",
                principalTable: "GiayChiTiets",
                principalColumn: "GiayChiTietId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TraHangs_HoaDons_HoaDonId",
                table: "TraHangs",
                column: "HoaDonId",
                principalTable: "HoaDons",
                principalColumn: "HoaDonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TraHangs_GiayChiTiets_GiayChiTietId",
                table: "TraHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_TraHangs_HoaDons_HoaDonId",
                table: "TraHangs");

            migrationBuilder.CreateTable(
                name: "CuocTroChuyenAIs",
                columns: table => new
                {
                    CuocTroChuyenAIId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KhachHangId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DaXoa = table.Column<bool>(type: "bit", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhienKhachId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TieuDe = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuocTroChuyenAIs", x => x.CuocTroChuyenAIId);
                    table.ForeignKey(
                        name: "FK_CuocTroChuyenAIs_KhachHangs_KhachHangId",
                        column: x => x.KhachHangId,
                        principalTable: "KhachHangs",
                        principalColumn: "KhachHangId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TinNhanAIs",
                columns: table => new
                {
                    TinNhanAIId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CuocTroChuyenAIId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelAI = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhanAIs", x => x.TinNhanAIId);
                    table.ForeignKey(
                        name: "FK_TinNhanAIs_CuocTroChuyenAIs_CuocTroChuyenAIId",
                        column: x => x.CuocTroChuyenAIId,
                        principalTable: "CuocTroChuyenAIs",
                        principalColumn: "CuocTroChuyenAIId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "hinhThucThanhToans",
                columns: new[] { "HinhThucThanhToanId", "MoTa", "TenHinhThuc", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Khách thanh toán khi nhận hàng", "Thanh toán khi nhận hàng (COD)", true },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Thanh toán online", "Momo", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuocTroChuyenAIs_KhachHangId_NgayCapNhat",
                table: "CuocTroChuyenAIs",
                columns: new[] { "KhachHangId", "NgayCapNhat" });

            migrationBuilder.CreateIndex(
                name: "IX_CuocTroChuyenAIs_PhienKhachId_NgayCapNhat",
                table: "CuocTroChuyenAIs",
                columns: new[] { "PhienKhachId", "NgayCapNhat" });

            migrationBuilder.CreateIndex(
                name: "IX_TinNhanAIs_CuocTroChuyenAIId_ThoiGian",
                table: "TinNhanAIs",
                columns: new[] { "CuocTroChuyenAIId", "ThoiGian" });

            migrationBuilder.AddForeignKey(
                name: "FK_TraHangs_GiayChiTiets_GiayChiTietId",
                table: "TraHangs",
                column: "GiayChiTietId",
                principalTable: "GiayChiTiets",
                principalColumn: "GiayChiTietId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TraHangs_HoaDons_HoaDonId",
                table: "TraHangs",
                column: "HoaDonId",
                principalTable: "HoaDons",
                principalColumn: "HoaDonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
