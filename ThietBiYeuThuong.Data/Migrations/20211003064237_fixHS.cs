using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class fixHS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CTPhieuNXes");

            migrationBuilder.DropTable(
                name: "PhieuNXes");

            migrationBuilder.CreateTable(
                name: "HoSoBNs",
                columns: table => new
                {
                    SoPhieu = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BenhNhanId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    LapPhieu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    HoTenNVYTe = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SDT_NVYT = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    DonVi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    STT = table.Column<int>(type: "int", nullable: false),
                    NVTruc = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoBNs", x => x.SoPhieu);
                    table.ForeignKey(
                        name: "FK_HoSoBNs_BenhNhans_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "BenhNhans",
                        principalColumn: "MaBN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTHoSoBNs",
                columns: table => new
                {
                    SoPhieuCT = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    HoSoBNId = table.Column<string>(type: "varchar(10)", nullable: false),
                    ThietBiId = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    LapPhieu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayXuat = table.Column<DateTime>(type: "datetime", nullable: true),
                    DongHoGiao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    DongHoThu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NVGiaoBinh = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    SoLuongHienTai = table.Column<int>(type: "int", nullable: false),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTHoSoBNs", x => x.SoPhieuCT);
                    table.ForeignKey(
                        name: "FK_CTHoSoBNs_HoSoBNs_HoSoBNId",
                        column: x => x.HoSoBNId,
                        principalTable: "HoSoBNs",
                        principalColumn: "SoPhieu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CTHoSoBNs_ThietBis_ThietBiId",
                        column: x => x.ThietBiId,
                        principalTable: "ThietBis",
                        principalColumn: "MaTB",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CTHoSoBNs_HoSoBNId",
                table: "CTHoSoBNs",
                column: "HoSoBNId");

            migrationBuilder.CreateIndex(
                name: "IX_CTHoSoBNs_ThietBiId",
                table: "CTHoSoBNs",
                column: "ThietBiId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoBNs_BenhNhanId",
                table: "HoSoBNs",
                column: "BenhNhanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CTHoSoBNs");

            migrationBuilder.DropTable(
                name: "HoSoBNs");

            migrationBuilder.CreateTable(
                name: "PhieuNXes",
                columns: table => new
                {
                    SoPhieu = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BenhNhanId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    DonVi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    HoTenNVYTe = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LapPhieu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LoaiPhieu = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    NVTruc = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    SDT_NVYT = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    STT = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNXes", x => x.SoPhieu);
                    table.ForeignKey(
                        name: "FK_PhieuNXes_BenhNhans_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "BenhNhans",
                        principalColumn: "MaBN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTPhieuNXes",
                columns: table => new
                {
                    SoPhieuCT = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    DongHoGiao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    DongHoThu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LapPhieu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    NVGiaoBinh = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayXuat = table.Column<DateTime>(type: "datetime", nullable: true),
                    PhieuNXId = table.Column<string>(type: "varchar(10)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    SoLuongHienTai = table.Column<int>(type: "int", nullable: false),
                    ThietBiId = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPhieuNXes", x => x.SoPhieuCT);
                    table.ForeignKey(
                        name: "FK_CTPhieuNXes_PhieuNXes_PhieuNXId",
                        column: x => x.PhieuNXId,
                        principalTable: "PhieuNXes",
                        principalColumn: "SoPhieu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CTPhieuNXes_ThietBis_ThietBiId",
                        column: x => x.ThietBiId,
                        principalTable: "ThietBis",
                        principalColumn: "MaTB",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CTPhieuNXes_PhieuNXId",
                table: "CTPhieuNXes",
                column: "PhieuNXId");

            migrationBuilder.CreateIndex(
                name: "IX_CTPhieuNXes_ThietBiId",
                table: "CTPhieuNXes",
                column: "ThietBiId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNXes_BenhNhanId",
                table: "PhieuNXes",
                column: "BenhNhanId");
        }
    }
}
