using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class addNVYTTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonVi",
                table: "HoSoBNs");

            migrationBuilder.DropColumn(
                name: "HoTenNVYTe",
                table: "HoSoBNs");

            migrationBuilder.DropColumn(
                name: "SDT_NVYT",
                table: "HoSoBNs");

            migrationBuilder.AddColumn<string>(
                name: "MaNVYT",
                table: "HoSoBNs",
                type: "varchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NhanVienYTes",
                columns: table => new
                {
                    MaNVYT = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    HoTenNVYTe = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SDT_NVYT = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    DonVi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NguoiTao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiSua = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogFile = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVienYTes", x => x.MaNVYT);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoSoBNs_MaNVYT",
                table: "HoSoBNs",
                column: "MaNVYT");

            migrationBuilder.AddForeignKey(
                name: "FK_HoSoBNs_NhanVienYTes_MaNVYT",
                table: "HoSoBNs",
                column: "MaNVYT",
                principalTable: "NhanVienYTes",
                principalColumn: "MaNVYT",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoSoBNs_NhanVienYTes_MaNVYT",
                table: "HoSoBNs");

            migrationBuilder.DropTable(
                name: "NhanVienYTes");

            migrationBuilder.DropIndex(
                name: "IX_HoSoBNs_MaNVYT",
                table: "HoSoBNs");

            migrationBuilder.DropColumn(
                name: "MaNVYT",
                table: "HoSoBNs");

            migrationBuilder.AddColumn<string>(
                name: "DonVi",
                table: "HoSoBNs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTenNVYTe",
                table: "HoSoBNs",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SDT_NVYT",
                table: "HoSoBNs",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
