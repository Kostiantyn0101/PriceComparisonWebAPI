using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class ProductCharacteristicDBModelValueqIndexAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_ProductCharacteristic_AtLeastOneValue",
                table: "ProductCharacteristics",
                sql: "(ValueText IS NOT NULL) OR (ValueNumber IS NOT NULL) OR (ValueBoolean IS NOT NULL) OR (ValueDate IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ProductCharacteristic_AtLeastOneValue",
                table: "ProductCharacteristics");
        }
    }
}
