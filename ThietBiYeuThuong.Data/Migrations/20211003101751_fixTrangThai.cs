using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class fixTrangThai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ThietBis_TrangThaiId",
                table: "ThietBis",
                column: "TrangThaiId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThietBis_TrangThais_TrangThaiId",
                table: "ThietBis",
                column: "TrangThaiId",
                principalTable: "TrangThais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThietBis_TrangThais_TrangThaiId",
                table: "ThietBis");

            migrationBuilder.DropIndex(
                name: "IX_ThietBis_TrangThaiId",
                table: "ThietBis");
        }
    }
}
