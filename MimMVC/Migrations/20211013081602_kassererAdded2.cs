using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class kassererAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AspNetUsers_ApplicationUserId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Invoices",
                newName: "ApplicationUser.FullName");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ApplicationUserId",
                table: "Invoices",
                newName: "IX_Invoices_ApplicationUser.FullName");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserFullName",
                table: "Invoices",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AspNetUsers_ApplicationUser.FullName",
                table: "Invoices",
                column: "ApplicationUser.FullName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AspNetUsers_ApplicationUser.FullName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ApplicationUserFullName",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "ApplicationUser.FullName",
                table: "Invoices",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ApplicationUser.FullName",
                table: "Invoices",
                newName: "IX_Invoices_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AspNetUsers_ApplicationUserId",
                table: "Invoices",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
