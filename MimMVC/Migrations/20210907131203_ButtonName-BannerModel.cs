using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class ButtonNameBannerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ButtonName",
                table: "Banners",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonName",
                table: "Banners");
        }
    }
}
