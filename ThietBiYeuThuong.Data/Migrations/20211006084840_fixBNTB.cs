using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class fixBNTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "BenhNhanThietBis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NguoiTao",
                table: "BenhNhanThietBis",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "BenhNhanThietBis");

            migrationBuilder.DropColumn(
                name: "NguoiTao",
                table: "BenhNhanThietBis");
        }
    }
}
