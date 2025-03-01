using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ClickTrackingsTableChangedAndRenaimed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClickTrackings_Products_ProductId",
                table: "ClickTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ClickTrackings_Sellers_SellerId",
                table: "ClickTrackings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClickTrackings",
                table: "ClickTrackings");

            migrationBuilder.RenameTable(
                name: "ClickTrackings",
                newName: "ProductSellerReferenceClicks");

            migrationBuilder.RenameIndex(
                name: "IX_ClickTrackings_SellerId",
                table: "ProductSellerReferenceClicks",
                newName: "IX_ProductSellerReferenceClicks_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_ClickTrackings_ProductId",
                table: "ProductSellerReferenceClicks",
                newName: "IX_ProductSellerReferenceClicks_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSellerReferenceClicks",
                table: "ProductSellerReferenceClicks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellerReferenceClicks_Products_ProductId",
                table: "ProductSellerReferenceClicks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellerReferenceClicks_Sellers_SellerId",
                table: "ProductSellerReferenceClicks",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellerReferenceClicks_Products_ProductId",
                table: "ProductSellerReferenceClicks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellerReferenceClicks_Sellers_SellerId",
                table: "ProductSellerReferenceClicks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSellerReferenceClicks",
                table: "ProductSellerReferenceClicks");

            migrationBuilder.RenameTable(
                name: "ProductSellerReferenceClicks",
                newName: "ClickTrackings");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSellerReferenceClicks_SellerId",
                table: "ClickTrackings",
                newName: "IX_ClickTrackings_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSellerReferenceClicks_ProductId",
                table: "ClickTrackings",
                newName: "IX_ClickTrackings_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClickTrackings",
                table: "ClickTrackings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClickTrackings_Products_ProductId",
                table: "ClickTrackings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClickTrackings_Sellers_SellerId",
                table: "ClickTrackings",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
