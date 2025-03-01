using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ProductSellerLinksTableDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClickTrackings_ProductSellerLinks_ProductSellerLinkId",
                table: "ClickTrackings");

            migrationBuilder.DropTable(
                name: "ProductSellerLinks");

            migrationBuilder.DropIndex(
                name: "IX_ClickTrackings_ProductSellerLinkId",
                table: "ClickTrackings");

            migrationBuilder.DropColumn(
                name: "ProductSellerLinkId",
                table: "ClickTrackings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductSellerLinkId",
                table: "ClickTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductSellerLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SellerDBModelId = table.Column<int>(type: "int", nullable: true),
                    SellerUrl = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSellerLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSellerLinks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSellerLinks_Sellers_SellerDBModelId",
                        column: x => x.SellerDBModelId,
                        principalTable: "Sellers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClickTrackings_ProductSellerLinkId",
                table: "ClickTrackings",
                column: "ProductSellerLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSellerLinks_ProductId",
                table: "ProductSellerLinks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSellerLinks_SellerDBModelId",
                table: "ProductSellerLinks",
                column: "SellerDBModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClickTrackings_ProductSellerLinks_ProductSellerLinkId",
                table: "ClickTrackings",
                column: "ProductSellerLinkId",
                principalTable: "ProductSellerLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
