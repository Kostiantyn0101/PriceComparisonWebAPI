using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class AddMonetisation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellerLinkDBModel_Products_ProductId",
                table: "ProductSellerLinkDBModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSellerLinkDBModel",
                table: "ProductSellerLinkDBModel");

            migrationBuilder.RenameTable(
                name: "ProductSellerLinkDBModel",
                newName: "ProductSellerLinks");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSellerLinkDBModel_ProductId",
                table: "ProductSellerLinks",
                newName: "IX_ProductSellerLinks_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "SellerDBModelId",
                table: "ProductSellerLinks",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSellerLinks",
                table: "ProductSellerLinks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClickTrackings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    ProductSellerLinkId = table.Column<int>(type: "int", nullable: false),
                    ClickedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClickTrackings_ProductSellerLinks_ProductSellerLinkId",
                        column: x => x.ProductSellerLinkId,
                        principalTable: "ProductSellerLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClickTrackings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClickTrackings_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MonthlyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellerPaymentPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    PaymentPlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerPaymentPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerPaymentPlans_PaymentPlans_PaymentPlanId",
                        column: x => x.PaymentPlanId,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellerPaymentPlans_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSellerLinks_SellerDBModelId",
                table: "ProductSellerLinks",
                column: "SellerDBModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ClickTrackings_ProductId",
                table: "ClickTrackings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ClickTrackings_ProductSellerLinkId",
                table: "ClickTrackings",
                column: "ProductSellerLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_ClickTrackings_SellerId",
                table: "ClickTrackings",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerPaymentPlans_PaymentPlanId",
                table: "SellerPaymentPlans",
                column: "PaymentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerPaymentPlans_SellerId",
                table: "SellerPaymentPlans",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellerLinks_Products_ProductId",
                table: "ProductSellerLinks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellerLinks_Sellers_SellerDBModelId",
                table: "ProductSellerLinks",
                column: "SellerDBModelId",
                principalTable: "Sellers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellerLinks_Products_ProductId",
                table: "ProductSellerLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellerLinks_Sellers_SellerDBModelId",
                table: "ProductSellerLinks");

            migrationBuilder.DropTable(
                name: "ClickTrackings");

            migrationBuilder.DropTable(
                name: "SellerPaymentPlans");

            migrationBuilder.DropTable(
                name: "PaymentPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSellerLinks",
                table: "ProductSellerLinks");

            migrationBuilder.DropIndex(
                name: "IX_ProductSellerLinks_SellerDBModelId",
                table: "ProductSellerLinks");

            migrationBuilder.DropColumn(
                name: "SellerDBModelId",
                table: "ProductSellerLinks");

            migrationBuilder.RenameTable(
                name: "ProductSellerLinks",
                newName: "ProductSellerLinkDBModel");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSellerLinks_ProductId",
                table: "ProductSellerLinkDBModel",
                newName: "IX_ProductSellerLinkDBModel_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSellerLinkDBModel",
                table: "ProductSellerLinkDBModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellerLinkDBModel_Products_ProductId",
                table: "ProductSellerLinkDBModel",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
