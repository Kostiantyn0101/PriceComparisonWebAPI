using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ColorsTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColorDBModel_Products_ProductDBModelId",
                table: "ColorDBModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ColorDBModel",
                table: "ColorDBModel");

            migrationBuilder.DropIndex(
                name: "IX_ColorDBModel_ProductDBModelId",
                table: "ColorDBModel");

            migrationBuilder.DropColumn(
                name: "ProductDBModelId",
                table: "ColorDBModel");

            migrationBuilder.RenameTable(
                name: "ColorDBModel",
                newName: "Colors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colors",
                table: "Colors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ColorId",
                table: "Products",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Colors_ColorId",
                table: "Products",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Colors_ColorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ColorId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colors",
                table: "Colors");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "ColorDBModel");

            migrationBuilder.AddColumn<int>(
                name: "ProductDBModelId",
                table: "ColorDBModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ColorDBModel",
                table: "ColorDBModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ColorDBModel_ProductDBModelId",
                table: "ColorDBModel",
                column: "ProductDBModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ColorDBModel_Products_ProductDBModelId",
                table: "ColorDBModel",
                column: "ProductDBModelId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
