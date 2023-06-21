using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class kassererAdded3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Invoices",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Invoices",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Invoices",
                newName: "Name");
        }
    }
}
