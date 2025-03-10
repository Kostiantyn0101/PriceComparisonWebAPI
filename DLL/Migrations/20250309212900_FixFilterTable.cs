using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class FixFilterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilterCriteria");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Filters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CharacteristicId",
                table: "Filters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisplayOnProduct",
                table: "Filters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Operator",
                table: "Filters",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Filters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_CharacteristicId",
                table: "Filters",
                column: "CharacteristicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Filters_Characteristics_CharacteristicId",
                table: "Filters",
                column: "CharacteristicId",
                principalTable: "Characteristics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filters_Characteristics_CharacteristicId",
                table: "Filters");

            migrationBuilder.DropIndex(
                name: "IX_Filters_CharacteristicId",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "CharacteristicId",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "DisplayOnProduct",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "Operator",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Filters");

            migrationBuilder.CreateTable(
                name: "FilterCriteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacteristicId = table.Column<int>(type: "int", nullable: false),
                    FilterId = table.Column<int>(type: "int", nullable: false),
                    Operator = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterCriteria_Characteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "Characteristics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FilterCriteria_Filters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "Filters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilterCriteria_CharacteristicId",
                table: "FilterCriteria",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterCriteria_FilterId",
                table: "FilterCriteria",
                column: "FilterId");
        }
    }
}
