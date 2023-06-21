using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class UserInput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SNN",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Dob",
                table: "AspNetUsers",
                newName: "DateBorn");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "AspNetUsers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PostalCode",
                table: "AspNetUsers",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "DateBorn",
                table: "AspNetUsers",
                newName: "Dob");

            migrationBuilder.AddColumn<int>(
                name: "SNN",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
