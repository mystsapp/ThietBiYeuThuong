using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class addNewTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CTPhieus",
                columns: table => new
                {
                    SoPhieuCT = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    SoPhieu = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
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
                    table.PrimaryKey("PK_CTPhieus", x => x.SoPhieuCT);
                    table.ForeignKey(
                        name: "FK_CTPhieus_ThietBis_ThietBiId",
                        column: x => x.ThietBiId,
                        principalTable: "ThietBis",
                        principalColumn: "MaTB",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhaps",
                columns: table => new
                {
                    SoPhieu = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
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
                name: "PhieuXuats",
                columns: table => new
                {
                    SoPhieu = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_CTPhieus_ThietBiId",
                table: "CTPhieus",
                column: "ThietBiId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuats_BenhNhanId",
                table: "PhieuXuats",
                column: "BenhNhanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CTPhieus");

            migrationBuilder.DropTable(
                name: "PhieuNhaps");

            migrationBuilder.DropTable(
                name: "PhieuXuats");
        }
    }
}
