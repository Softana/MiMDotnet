using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class ForeldresNavne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentsName",
                table: "AspNetUsers",
                newName: "ParentsLastName");

            migrationBuilder.AddColumn<string>(
                name: "ParentsFirstName",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentsFirstName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ParentsLastName",
                table: "AspNetUsers",
                newName: "ParentsName");
        }
    }
}
