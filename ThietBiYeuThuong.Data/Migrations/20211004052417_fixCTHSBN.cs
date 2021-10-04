using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class fixCTHSBN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CTHoSoBNs_ThietBis_ThietBiId",
                table: "CTHoSoBNs");

            migrationBuilder.DropIndex(
                name: "IX_CTHoSoBNs_ThietBiId",
                table: "CTHoSoBNs");

            migrationBuilder.DropColumn(
                name: "ThietBiId",
                table: "CTHoSoBNs");

            migrationBuilder.AddColumn<string>(
                name: "ThietBi",
                table: "CTHoSoBNs",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThietBi",
                table: "CTHoSoBNs");

            migrationBuilder.AddColumn<string>(
                name: "ThietBiId",
                table: "CTHoSoBNs",
                type: "varchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CTHoSoBNs_ThietBiId",
                table: "CTHoSoBNs",
                column: "ThietBiId");

            migrationBuilder.AddForeignKey(
                name: "FK_CTHoSoBNs_ThietBis_ThietBiId",
                table: "CTHoSoBNs",
                column: "ThietBiId",
                principalTable: "ThietBis",
                principalColumn: "MaTB",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
