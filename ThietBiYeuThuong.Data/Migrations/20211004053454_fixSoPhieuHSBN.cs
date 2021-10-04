using Microsoft.EntityFrameworkCore.Migrations;

namespace ThietBiYeuThuong.Data.Migrations
{
    public partial class fixSoPhieuHSBN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CTHoSoBNs_HoSoBNs_HoSoBNId",
                table: "CTHoSoBNs");

            migrationBuilder.AlterColumn<string>(
                name: "SoPhieu",
                table: "HoSoBNs",
                type: "varchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "HoSoBNId",
                table: "CTHoSoBNs",
                type: "varchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)");

            migrationBuilder.AlterColumn<string>(
                name: "SoPhieuCT",
                table: "CTHoSoBNs",
                type: "varchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddForeignKey(
                name: "FK_CTHoSoBNs_HoSoBNs_HoSoBNId",
                table: "CTHoSoBNs",
                column: "HoSoBNId",
                principalTable: "HoSoBNs",
                principalColumn: "SoPhieu",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CTHoSoBNs_HoSoBNs_HoSoBNId",
                table: "CTHoSoBNs");

            migrationBuilder.AlterColumn<string>(
                name: "SoPhieu",
                table: "HoSoBNs",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "HoSoBNId",
                table: "CTHoSoBNs",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoPhieuCT",
                table: "CTHoSoBNs",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AddForeignKey(
                name: "FK_CTHoSoBNs_HoSoBNs_HoSoBNId",
                table: "CTHoSoBNs",
                column: "HoSoBNId",
                principalTable: "HoSoBNs",
                principalColumn: "SoPhieu",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
