using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MimMVC.Migrations
{
    public partial class Skyggetabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserWorkShop");

            migrationBuilder.CreateTable(
                name: "WorkShopIndmeldelser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gæster = table.Column<int>(type: "int", nullable: false),
                    WorkShopId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShopIndmeldelser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkShopIndmeldelser_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkShopIndmeldelser_Workshops_WorkShopId",
                        column: x => x.WorkShopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShopIndmeldelser_userId",
                table: "WorkShopIndmeldelser",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShopIndmeldelser_WorkShopId",
                table: "WorkShopIndmeldelser",
                column: "WorkShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkShopIndmeldelser");

            migrationBuilder.CreateTable(
                name: "ApplicationUserWorkShop",
                columns: table => new
                {
                    TilmeldteBrugereId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TilmeldteWorkshopsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserWorkShop", x => new { x.TilmeldteBrugereId, x.TilmeldteWorkshopsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserWorkShop_AspNetUsers_TilmeldteBrugereId",
                        column: x => x.TilmeldteBrugereId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserWorkShop_Workshops_TilmeldteWorkshopsId",
                        column: x => x.TilmeldteWorkshopsId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserWorkShop_TilmeldteWorkshopsId",
                table: "ApplicationUserWorkShop",
                column: "TilmeldteWorkshopsId");
        }
    }
}
