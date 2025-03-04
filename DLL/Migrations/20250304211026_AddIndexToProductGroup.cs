using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToProductGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_ProductId",
                table: "ProductGroups");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ProductId_ProductGroupId",
                table: "ProductGroups",
                columns: new[] { "ProductId", "ProductGroupId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_ProductId_ProductGroupId",
                table: "ProductGroups");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ProductId",
                table: "ProductGroups",
                column: "ProductId");
        }
    }
}
