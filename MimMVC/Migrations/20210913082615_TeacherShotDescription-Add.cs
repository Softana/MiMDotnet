using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class TeacherShotDescriptionAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeacherShotDescription",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherShotDescription",
                table: "AspNetUsers");
        }
    }
}
