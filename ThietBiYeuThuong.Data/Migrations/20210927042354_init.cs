using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhieuNXes",
                columns: table => new
                {
                    SoPhieu = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    LoaiPhieu = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    LapPhieu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    STT = table.Column<int>(type: "int", nullable: false),
                    NVTruc = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    HoTenTN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SDT_TN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    GT_TN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HoTenBN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NamSinh = table.Column<int>(type: "int", nullable: false),
                    CMND_CCCD_BN = table.Column<int>(type: "int", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    HoTenNVYTe = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SDT_NVYT = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    DonVi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TinhTrangBN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BenhNenBN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChiSoSPO2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TinhTrangBNSauO2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    KetLuan = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNXes", x => x.SoPhieu);
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
                    TenTB = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    NguoiTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinhTons", x => x.Id);
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
                name: "CTPhieuNXes",
                columns: table => new
                {
                    SoPhieuCT = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    PhieuNXId = table.Column<string>(type: "varchar(10)", nullable: false),
                    ThietBi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LapPhieu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime", nullable: true),
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
                    table.PrimaryKey("PK_CTPhieuNXes", x => x.SoPhieuCT);
                    table.ForeignKey(
                        name: "FK_CTPhieuNXes_PhieuNXes_PhieuNXId",
                        column: x => x.PhieuNXId,
                        principalTable: "PhieuNXes",
                        principalColumn: "SoPhieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CTPhieuNXes_PhieuNXId",
                table: "CTPhieuNXes",
                column: "PhieuNXId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CTPhieuNXes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TinhTons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PhieuNXes");
        }
    }
}
