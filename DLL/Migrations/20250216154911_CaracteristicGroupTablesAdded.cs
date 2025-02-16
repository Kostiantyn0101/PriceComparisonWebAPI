using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class CaracteristicGroupTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacteristicGroupId",
                table: "Characteristics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Characteristics",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeInShortDescription",
                table: "Characteristics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CharacteristicGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCharacteristicGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CharacteristicGroupId = table.Column<int>(type: "int", nullable: false),
                    GroupDisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCharacteristicGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryCharacteristicGroups_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryCharacteristicGroups_CharacteristicGroups_CharacteristicGroupId",
                        column: x => x.CharacteristicGroupId,
                        principalTable: "CharacteristicGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_CharacteristicGroupId",
                table: "Characteristics",
                column: "CharacteristicGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCharacteristicGroups_CategoryId",
                table: "CategoryCharacteristicGroups",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCharacteristicGroups_CharacteristicGroupId",
                table: "CategoryCharacteristicGroups",
                column: "CharacteristicGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristics_CharacteristicGroups_CharacteristicGroupId",
                table: "Characteristics",
                column: "CharacteristicGroupId",
                principalTable: "CharacteristicGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characteristics_CharacteristicGroups_CharacteristicGroupId",
                table: "Characteristics");

            migrationBuilder.DropTable(
                name: "CategoryCharacteristicGroups");

            migrationBuilder.DropTable(
                name: "CharacteristicGroups");

            migrationBuilder.DropIndex(
                name: "IX_Characteristics_CharacteristicGroupId",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "CharacteristicGroupId",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "IncludeInShortDescription",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Categories");
        }
    }
}
