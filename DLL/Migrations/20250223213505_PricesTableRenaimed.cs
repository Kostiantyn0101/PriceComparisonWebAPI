using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class PricesTableRenaimed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Products_ProductId",
                table: "Prices");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Sellers_SellerId",
                table: "Prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prices",
                table: "Prices");

            migrationBuilder.RenameTable(
                name: "Prices",
                newName: "SellerProductDetails");

            migrationBuilder.RenameColumn(
                name: "AccoundBalance",
                table: "Sellers",
                newName: "AccountBalance");

            migrationBuilder.RenameIndex(
                name: "IX_Prices_SellerId",
                table: "SellerProductDetails",
                newName: "IX_SellerProductDetails_SellerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerProductDetails",
                table: "SellerProductDetails",
                columns: new[] { "ProductId", "SellerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProductDetails_Products_ProductId",
                table: "SellerProductDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProductDetails_Sellers_SellerId",
                table: "SellerProductDetails",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerProductDetails_Products_ProductId",
                table: "SellerProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerProductDetails_Sellers_SellerId",
                table: "SellerProductDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerProductDetails",
                table: "SellerProductDetails");

            migrationBuilder.RenameTable(
                name: "SellerProductDetails",
                newName: "Prices");

            migrationBuilder.RenameColumn(
                name: "AccountBalance",
                table: "Sellers",
                newName: "AccoundBalance");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProductDetails_SellerId",
                table: "Prices",
                newName: "IX_Prices_SellerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prices",
                table: "Prices",
                columns: new[] { "ProductId", "SellerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Products_ProductId",
                table: "Prices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Sellers_SellerId",
                table: "Prices",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
