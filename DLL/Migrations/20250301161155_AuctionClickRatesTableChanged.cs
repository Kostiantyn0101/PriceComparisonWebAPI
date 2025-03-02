using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class AuctionClickRatesTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionClickRates_Products_ProductId",
                table: "AuctionClickRates");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AuctionClickRates",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionClickRates_ProductId",
                table: "AuctionClickRates",
                newName: "IX_AuctionClickRates_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionClickRates_Categories_CategoryId",
                table: "AuctionClickRates",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionClickRates_Categories_CategoryId",
                table: "AuctionClickRates");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "AuctionClickRates",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionClickRates_CategoryId",
                table: "AuctionClickRates",
                newName: "IX_AuctionClickRates_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionClickRates_Products_ProductId",
                table: "AuctionClickRates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
