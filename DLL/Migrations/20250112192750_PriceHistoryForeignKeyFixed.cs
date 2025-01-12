using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class PriceHistoryForeignKeyFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PricesHistory_Products_ProductDBModelId",
                table: "PricesHistory");

            migrationBuilder.DropIndex(
                name: "IX_PricesHistory_ProductDBModelId",
                table: "PricesHistory");

            migrationBuilder.DropColumn(
                name: "ProductDBModelId",
                table: "PricesHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductDBModelId",
                table: "PricesHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PricesHistory_ProductDBModelId",
                table: "PricesHistory",
                column: "ProductDBModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PricesHistory_Products_ProductDBModelId",
                table: "PricesHistory",
                column: "ProductDBModelId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
