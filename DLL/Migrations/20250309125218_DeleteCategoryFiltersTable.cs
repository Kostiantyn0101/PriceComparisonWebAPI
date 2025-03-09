using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCategoryFiltersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryFilterCriteria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryFilterCriteria",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    FilterCriterionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryFilterCriteria", x => new { x.CategoryId, x.FilterCriterionId });
                    table.ForeignKey(
                        name: "FK_CategoryFilterCriteria_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryFilterCriteria_FilterCriteria_FilterCriterionId",
                        column: x => x.FilterCriterionId,
                        principalTable: "FilterCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFilterCriteria_FilterCriterionId",
                table: "CategoryFilterCriteria",
                column: "FilterCriterionId");
        }
    }
}
