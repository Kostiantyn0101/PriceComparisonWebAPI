using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedFieldsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedModelNumber",
                table: "Products",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductGroups",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "ProductGroups",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Product_NormalizedModelNumber",
                table: "Products",
                column: "NormalizedModelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroup_NormalizedName",
                table: "ProductGroups",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_BaseProduct_NormalizedTitle",
                table: "BaseProducts",
                column: "NormalizedTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_NormalizedModelNumber",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductGroup_NormalizedName",
                table: "ProductGroups");

            migrationBuilder.DropIndex(
                name: "IX_BaseProduct_NormalizedTitle",
                table: "BaseProducts");

            migrationBuilder.DropColumn(
                name: "NormalizedModelNumber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "ProductGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductGroups",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
