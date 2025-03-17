using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ProductGroupTypesTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductGroupTypeId",
                table: "ProductGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductGroupTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroupTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ProductGroupTypeId",
                table: "ProductGroups",
                column: "ProductGroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroupTypes_Name",
                table: "ProductGroupTypes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroups_ProductGroupTypes_ProductGroupTypeId",
                table: "ProductGroups",
                column: "ProductGroupTypeId",
                principalTable: "ProductGroupTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroups_ProductGroupTypes_ProductGroupTypeId",
                table: "ProductGroups");

            migrationBuilder.DropTable(
                name: "ProductGroupTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_ProductGroupTypeId",
                table: "ProductGroups");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductGroups");

            migrationBuilder.DropColumn(
                name: "ProductGroupTypeId",
                table: "ProductGroups");
        }
    }
}
