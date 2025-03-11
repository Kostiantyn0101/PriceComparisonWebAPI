using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ProductCharacteristicsTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCharacteristics_Products_ProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCharacteristics",
                table: "ProductCharacteristics");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductCharacteristics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductCharacteristics",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCharacteristics",
                table: "ProductCharacteristics",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCharacteristics_BaseProductId",
                table: "ProductCharacteristics",
                column: "BaseProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCharacteristics_ProductId",
                table: "ProductCharacteristics",
                column: "ProductId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ProductCharacteristic_AtLeastOneProduct",
                table: "ProductCharacteristics",
                sql: "(ProductId IS NOT NULL) OR (BaseProductId IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCharacteristics_BaseProducts_BaseProductId",
                table: "ProductCharacteristics",
                column: "BaseProductId",
                principalTable: "BaseProducts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCharacteristics_Products_ProductId",
                table: "ProductCharacteristics",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCharacteristics_BaseProducts_BaseProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCharacteristics_Products_ProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCharacteristics",
                table: "ProductCharacteristics");

            migrationBuilder.DropIndex(
                name: "IX_ProductCharacteristics_BaseProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropIndex(
                name: "IX_ProductCharacteristics_ProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ProductCharacteristic_AtLeastOneProduct",
                table: "ProductCharacteristics");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductCharacteristics");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductCharacteristics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCharacteristics",
                table: "ProductCharacteristics",
                columns: new[] { "ProductId", "CharacteristicId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCharacteristics_Products_ProductId",
                table: "ProductCharacteristics",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
