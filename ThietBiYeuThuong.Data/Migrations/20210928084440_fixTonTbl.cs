using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class fixTonTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoLuongNhap",
                table: "TinhTons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoLuongXuat",
                table: "TinhTons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoLuongNhap",
                table: "TinhTons");

            migrationBuilder.DropColumn(
                name: "SoLuongXuat",
                table: "TinhTons");
        }
    }
}
