using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenhNhans",
                columns: table => new
                {
                    MaBN = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    HoTenTN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SDT_TN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    GT_TN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HoTenBN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NamSinh = table.Column<int>(type: "int", nullable: false),
                    CMND_CCCD_BN = table.Column<int>(type: "int", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenhNhans", x => x.MaBN);
                });

            migrationBuilder.CreateTable(
                name: "LoaiThietBis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descripttion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiThietBis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhaps",
                columns: table => new
                {
                    SoPhieu = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    DonVi = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NguoiNhap = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNhaps", x => x.SoPhieu);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trangthai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TinhTons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayCT = table.Column<DateTime>(type: "datetime", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    SoLuongNhap = table.Column<int>(type: "int", nullable: false),
                    SoLuongXuat = table.Column<int>(type: "int", nullable: false),
                    TenTB = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    NguoiTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    SLNhap_Day = table.Column<int>(type: "int", nullable: false),
                    SLNhap_ThuHoi = table.Column<int>(type: "int", nullable: false),
                    SLNhap_VuaBomVe = table.Column<int>(type: "int", nullable: false),
                    SLXuat_GoiBom = table.Column<int>(type: "int", nullable: false),
                    SLXuat_BenhNhan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinhTons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrangThais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hoten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dienthoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Maphong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Macn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trangthai = table.Column<bool>(type: "bit", nullable: false),
                    Ngaytao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nguoitao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

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
                name: "PhieuXuats",
                columns: table => new
                {
                    SoPhieu = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    BenhNhanId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    NguoiXuat = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgayXuat = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuXuats", x => x.SoPhieu);
                    table.ForeignKey(
                        name: "FK_PhieuXuats_BenhNhans_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "BenhNhans",
                        principalColumn: "MaBN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TinhTrangBNs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BenhNhanId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BenhNenBN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChiSoSPO2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TinhTrangBNSauO2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    KetLuan = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NguoiTao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinhTrangBNs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TinhTrangBNs_BenhNhans_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "BenhNhans",
                        principalColumn: "MaBN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThietBis",
                columns: table => new
                {
                    MaTB = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    TenTB = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    TrangThaiId = table.Column<int>(type: "int", nullable: false),
                    LoaiTBId = table.Column<int>(type: "int", nullable: false),
                    TinhTrang = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietBis", x => x.MaTB);
                    table.ForeignKey(
                        name: "FK_ThietBis_LoaiThietBis_LoaiTBId",
                        column: x => x.LoaiTBId,
                        principalTable: "LoaiThietBis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThietBis_TrangThais_TrangThaiId",
                        column: x => x.TrangThaiId,
                        principalTable: "TrangThais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenhNhanThietBis",
                columns: table => new
                {
                    BenhNhanId = table.Column<string>(type: "varchar(12)", nullable: false),
                    ThietBiId = table.Column<string>(type: "varchar(12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenhNhanThietBis", x => new { x.BenhNhanId, x.ThietBiId });
                    table.ForeignKey(
                        name: "FK_BenhNhanThietBis_BenhNhans_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "BenhNhans",
                        principalColumn: "MaBN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenhNhanThietBis_ThietBis_ThietBiId",
                        column: x => x.ThietBiId,
                        principalTable: "ThietBis",
                        principalColumn: "MaTB",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CTHoSoBNs",
                columns: table => new
                {
                    SoPhieuCT = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    HoSoBNId = table.Column<string>(type: "varchar(10)", nullable: false),
                    ThietBiId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "CTPhieus",
                columns: table => new
                {
                    SoPhieuCT = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    SoPhieu = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    ThietBiId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
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
                    table.PrimaryKey("PK_CTPhieus", x => x.SoPhieuCT);
                    table.ForeignKey(
                        name: "FK_CTPhieus_ThietBis_ThietBiId",
                        column: x => x.ThietBiId,
                        principalTable: "ThietBis",
                        principalColumn: "MaTB",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenhNhanThietBis_ThietBiId",
                table: "BenhNhanThietBis",
                column: "ThietBiId");

            migrationBuilder.CreateIndex(
                name: "IX_CTHoSoBNs_HoSoBNId",
                table: "CTHoSoBNs",
                column: "HoSoBNId");

            migrationBuilder.CreateIndex(
                name: "IX_CTHoSoBNs_ThietBiId",
                table: "CTHoSoBNs",
                column: "ThietBiId");

            migrationBuilder.CreateIndex(
                name: "IX_CTPhieus_ThietBiId",
                table: "CTPhieus",
                column: "ThietBiId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoBNs_BenhNhanId",
                table: "HoSoBNs",
                column: "BenhNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuats_BenhNhanId",
                table: "PhieuXuats",
                column: "BenhNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBis_LoaiTBId",
                table: "ThietBis",
                column: "LoaiTBId");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBis_TrangThaiId",
                table: "ThietBis",
                column: "TrangThaiId");

            migrationBuilder.CreateIndex(
                name: "IX_TinhTrangBNs_BenhNhanId",
                table: "TinhTrangBNs",
                column: "BenhNhanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenhNhanThietBis");

            migrationBuilder.DropTable(
                name: "CTHoSoBNs");

            migrationBuilder.DropTable(
                name: "CTPhieus");

            migrationBuilder.DropTable(
                name: "PhieuNhaps");

            migrationBuilder.DropTable(
                name: "PhieuXuats");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TinhTons");

            migrationBuilder.DropTable(
                name: "TinhTrangBNs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "HoSoBNs");

            migrationBuilder.DropTable(
                name: "ThietBis");

            migrationBuilder.DropTable(
                name: "BenhNhans");

            migrationBuilder.DropTable(
                name: "LoaiThietBis");

            migrationBuilder.DropTable(
                name: "TrangThais");
        }
    }
}
