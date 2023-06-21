using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class TotalTilmeldte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkShopIndmeldelser_Workshops_WorkShopId",
                table: "WorkShopIndmeldelser");

            migrationBuilder.RenameColumn(
                name: "WorkShopId",
                table: "WorkShopIndmeldelser",
                newName: "workShopId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkShopIndmeldelser_WorkShopId",
                table: "WorkShopIndmeldelser",
                newName: "IX_WorkShopIndmeldelser_workShopId");

            migrationBuilder.AddColumn<int>(
                name: "TotalTilmeldt",
                table: "Workshops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkShopIndmeldelser_Workshops_workShopId",
                table: "WorkShopIndmeldelser",
                column: "workShopId",
                principalTable: "Workshops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkShopIndmeldelser_Workshops_workShopId",
                table: "WorkShopIndmeldelser");

            migrationBuilder.DropColumn(
                name: "TotalTilmeldt",
                table: "Workshops");

            migrationBuilder.RenameColumn(
                name: "workShopId",
                table: "WorkShopIndmeldelser",
                newName: "WorkShopId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkShopIndmeldelser_workShopId",
                table: "WorkShopIndmeldelser",
                newName: "IX_WorkShopIndmeldelser_WorkShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkShopIndmeldelser_Workshops_WorkShopId",
                table: "WorkShopIndmeldelser",
                column: "WorkShopId",
                principalTable: "Workshops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
