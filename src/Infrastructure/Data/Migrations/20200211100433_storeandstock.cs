using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.eShopWeb.Infrastructure.Data.Migrations
{
    public partial class storeandstock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Catalog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "Catalog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "BasketItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CatalogStores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogStores", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogStores");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Catalog");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "Catalog");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "BasketItems");
        }
    }
}
