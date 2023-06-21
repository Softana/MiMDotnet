using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class Studen_Tracher_OnApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Instruction",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "InteractWith",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NoYPlayed",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OneTeaching",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ParentsName",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TaughtBy",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Teach",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TeacherDescription",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Instruction",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InteractWith",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NoYPlayed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OneTeaching",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ParentsName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaughtBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Teach",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeacherDescription",
                table: "AspNetUsers");
        }
    }
}
