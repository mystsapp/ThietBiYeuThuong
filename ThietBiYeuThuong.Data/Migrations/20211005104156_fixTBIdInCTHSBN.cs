using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class fixTBIdInCTHSBN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
