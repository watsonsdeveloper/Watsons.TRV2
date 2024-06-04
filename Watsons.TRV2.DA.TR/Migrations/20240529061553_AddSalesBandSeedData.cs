using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Watsons.TRV2.DA.TR.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesBandSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SalesBand",
                columns: new[] { "SalesBandId", "CreatedBy", "SalesBand", "Type", "UpdatedAt", "UpdatedBy", "Value" },
                values: new object[,]
                {
                    { 1, "System", "A", "PLU_UNIT_LIMIT_OWN", null, null, 1m },
                    { 2, "System", "B", "PLU_UNIT_LIMIT_OWN", null, null, 1m },
                    { 3, "System", "C", "PLU_UNIT_LIMIT_OWN", null, null, 1m },
                    { 4, "System", "D", "PLU_UNIT_LIMIT_OWN", null, null, 1m },
                    { 5, "System", "E", "PLU_UNIT_LIMIT_OWN", null, null, 1m },
                    { 6, "System", "F", "PLU_UNIT_LIMIT_OWN", null, null, 1m },
                    { 7, "System", "A", "COST_LIMIT_OWN", null, null, 650m },
                    { 8, "System", "B", "COST_LIMIT_OWN", null, null, 650m },
                    { 9, "System", "C", "COST_LIMIT_OWN", null, null, 650m },
                    { 10, "System", "D", "COST_LIMIT_OWN", null, null, 550m },
                    { 11, "System", "E", "COST_LIMIT_OWN", null, null, 550m },
                    { 12, "System", "F", "COST_LIMIT_OWN", null, null, 550m },
                    { 13, "System", "A", "PLU_UNIT_LIMIT_SUPPLIER", null, null, 1m },
                    { 14, "System", "B", "PLU_UNIT_LIMIT_SUPPLIER", null, null, 1m },
                    { 15, "System", "C", "PLU_UNIT_LIMIT_SUPPLIER", null, null, 1m },
                    { 16, "System", "D", "PLU_UNIT_LIMIT_SUPPLIER", null, null, 1m },
                    { 17, "System", "E", "PLU_UNIT_LIMIT_SUPPLIER", null, null, 1m },
                    { 18, "System", "F", "PLU_UNIT_LIMIT_SUPPLIER", null, null, 1m },
                    { 19, "System", "A", "PLU_UNIT_LIMIT_COSMETIC", null, null, 2m },
                    { 20, "System", "B", "PLU_UNIT_LIMIT_COSMETIC", null, null, 1m },
                    { 21, "System", "C", "PLU_UNIT_LIMIT_COSMETIC", null, null, 1m },
                    { 22, "System", "D", "PLU_UNIT_LIMIT_COSMETIC", null, null, 1m },
                    { 23, "System", "E", "PLU_UNIT_LIMIT_COSMETIC", null, null, 1m },
                    { 24, "System", "F", "PLU_UNIT_LIMIT_COSMETIC", null, null, 1m },
                    { 25, "System", "A", "PLU_UNIT_LIMIT_SKINCARE", null, null, 2m },
                    { 26, "System", "B", "PLU_UNIT_LIMIT_SKINCARE", null, null, 1m },
                    { 27, "System", "C", "PLU_UNIT_LIMIT_SKINCARE", null, null, 1m },
                    { 28, "System", "D", "PLU_UNIT_LIMIT_SKINCARE", null, null, 1m },
                    { 29, "System", "E", "PLU_UNIT_LIMIT_SKINCARE", null, null, 1m },
                    { 30, "System", "F", "PLU_UNIT_LIMIT_SKINCARE", null, null, 1m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "SalesBand",
                keyColumn: "SalesBandId",
                keyValue: 30);
        }
    }
}
