using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ClickRateAddedToProductReferenceClicks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSellerReferenceClicks");

            migrationBuilder.CreateTable(
                name: "ProductReferenceClicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClickedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClickRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReferenceClicks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReferenceClicks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductReferenceClicks_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReferenceClicks_ProductId",
                table: "ProductReferenceClicks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReferenceClicks_SellerId",
                table: "ProductReferenceClicks",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductReferenceClicks");

            migrationBuilder.CreateTable(
                name: "ProductSellerReferenceClicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    ClickedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSellerReferenceClicks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSellerReferenceClicks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSellerReferenceClicks_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSellerReferenceClicks_ProductId",
                table: "ProductSellerReferenceClicks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSellerReferenceClicks_SellerId",
                table: "ProductSellerReferenceClicks",
                column: "SellerId");
        }
    }
}
