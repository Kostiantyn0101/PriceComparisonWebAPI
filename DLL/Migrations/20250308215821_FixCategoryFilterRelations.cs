using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoryFilterRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryFilterCriteria_Filters_FilterDBModelId",
                table: "CategoryFilterCriteria");

            migrationBuilder.DropIndex(
                name: "IX_CategoryFilterCriteria_FilterDBModelId",
                table: "CategoryFilterCriteria");

            migrationBuilder.DropColumn(
                name: "FilterDBModelId",
                table: "CategoryFilterCriteria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilterDBModelId",
                table: "CategoryFilterCriteria",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFilterCriteria_FilterDBModelId",
                table: "CategoryFilterCriteria",
                column: "FilterDBModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryFilterCriteria_Filters_FilterDBModelId",
                table: "CategoryFilterCriteria",
                column: "FilterDBModelId",
                principalTable: "Filters",
                principalColumn: "Id");
        }
    }
}
