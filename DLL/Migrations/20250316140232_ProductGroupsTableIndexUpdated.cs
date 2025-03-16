using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ProductGroupsTableIndexUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductGroupId_ColorId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id_ProductGroupId_ColorId",
                table: "Products",
                columns: new[] { "Id", "ProductGroupId", "ColorId" },
                unique: true,
                filter: "[ProductGroupId] IS NOT NULL AND [ColorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGroupId",
                table: "Products",
                column: "ProductGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Id_ProductGroupId_ColorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductGroupId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGroupId_ColorId",
                table: "Products",
                columns: new[] { "ProductGroupId", "ColorId" },
                unique: true,
                filter: "[ProductGroupId] IS NOT NULL AND [ColorId] IS NOT NULL");
        }
    }
}
