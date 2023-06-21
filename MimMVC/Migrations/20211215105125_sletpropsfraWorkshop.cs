using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class sletpropsfraWorkshop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gæster",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "InTime",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "OutTime",
                table: "Workshops");

            migrationBuilder.RenameColumn(
                name: "Paid",
                table: "Workshops",
                newName: "AntalGæster");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishTime",
                table: "Workshops",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Workshops",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Workshops");

            migrationBuilder.RenameColumn(
                name: "AntalGæster",
                table: "Workshops",
                newName: "Paid");

            migrationBuilder.AddColumn<int>(
                name: "Gæster",
                table: "Workshops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InTime",
                table: "Workshops",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OutTime",
                table: "Workshops",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
