using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class BaseProductIdToFeedbacksAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseProductId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BaseProductId",
                table: "Feedbacks",
                column: "BaseProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_BaseProducts_BaseProductId",
                table: "Feedbacks",
                column: "BaseProductId",
                principalTable: "BaseProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_BaseProducts_BaseProductId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_BaseProductId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "BaseProductId",
                table: "Feedbacks");
        }
    }
}
