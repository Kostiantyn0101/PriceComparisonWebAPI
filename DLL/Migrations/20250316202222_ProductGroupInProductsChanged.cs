using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ProductGroupInProductsChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Id_ProductGroupId_ColorId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductGroupId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id_ProductGroupId_ColorId",
                table: "Products",
                columns: new[] { "Id", "ProductGroupId", "ColorId" },
                unique: true,
                filter: "[ColorId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Id_ProductGroupId_ColorId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductGroupId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id_ProductGroupId_ColorId",
                table: "Products",
                columns: new[] { "Id", "ProductGroupId", "ColorId" },
                unique: true,
                filter: "[ProductGroupId] IS NOT NULL AND [ColorId] IS NOT NULL");
        }
    }
}
