using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class BaseProductsTableCreatedAndProductCharacteristicsTableIndexChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGroups");

            migrationBuilder.AddColumn<int>(
                name: "BaseProductId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseProductId",
                table: "ProductVideos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseProductId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VariantName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BaseProductId",
                table: "ProductCharacteristics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaseProductId",
                table: "Instructions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BaseProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NormalizedTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnderModeration = table.Column<bool>(type: "bit", nullable: false),
                    AddedToDatabase = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ColorDBModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HexCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradientCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDBModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorDBModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColorDBModel_Products_ProductDBModelId",
                        column: x => x.ProductDBModelId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BaseProductId",
                table: "Reviews",
                column: "BaseProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVideos_BaseProductId",
                table: "ProductVideos",
                column: "BaseProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BaseProductId",
                table: "Products",
                column: "BaseProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_BaseProductId",
                table: "Instructions",
                column: "BaseProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseProducts_CategoryId",
                table: "BaseProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ColorDBModel_ProductDBModelId",
                table: "ColorDBModel",
                column: "ProductDBModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_BaseProducts_BaseProductId",
                table: "Instructions",
                column: "BaseProductId",
                principalTable: "BaseProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_BaseProducts_BaseProductId",
                table: "Products",
                column: "BaseProductId",
                principalTable: "BaseProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVideos_BaseProducts_BaseProductId",
                table: "ProductVideos",
                column: "BaseProductId",
                principalTable: "BaseProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_BaseProducts_BaseProductId",
                table: "Reviews",
                column: "BaseProductId",
                principalTable: "BaseProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_BaseProducts_BaseProductId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_BaseProducts_BaseProductId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVideos_BaseProducts_BaseProductId",
                table: "ProductVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_BaseProducts_BaseProductId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "BaseProducts");

            migrationBuilder.DropTable(
                name: "ColorDBModel");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BaseProductId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductVideos_BaseProductId",
                table: "ProductVideos");

            migrationBuilder.DropIndex(
                name: "IX_Products_BaseProductId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Instructions_BaseProductId",
                table: "Instructions");

            migrationBuilder.DropColumn(
                name: "BaseProductId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BaseProductId",
                table: "ProductVideos");

            migrationBuilder.DropColumn(
                name: "BaseProductId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VariantName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BaseProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropColumn(
                name: "BaseProductId",
                table: "Instructions");

            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductGroupId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductGroups_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ProductId_ProductGroupId",
                table: "ProductGroups",
                columns: new[] { "ProductId", "ProductGroupId" },
                unique: true);
        }
    }
}
