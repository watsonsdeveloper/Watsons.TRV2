using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Watsons.TRV2.DA.TR.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__EnumLook__AB52D1D28856552A",
                table: "EnumLookUp");

            migrationBuilder.AddPrimaryKey(
                name: "PK__EnumLook__AB52D1D3A9AB9B15",
                table: "EnumLookUp",
                columns: new[] { "EnumName", "EnumId" });

            migrationBuilder.InsertData(
                table: "EnumLookUp",
                columns: new[] { "EnumId", "EnumName", "Description", "EnumValue" },
                values: new object[,]
                {
                    { 1, "Brand", null, "Own" },
                    { 2, "Brand", null, "Supplier" },
                    { 0, "HhtOrderStatus", null, "Error" },
                    { 1, "HhtOrderStatus", null, "Pending" },
                    { 2, "HhtOrderStatus", null, "Shipping" },
                    { 3, "HhtOrderStatus", null, "Shipped" },
                    { 4, "HhtOrderStatus", null, "Cancelled" },
                    { 5, "HhtOrderStatus", null, "Expire" },
                    { 6, "HhtOrderStatus", null, "StoreReceived" },
                    { 0, "Reason", null, "None" },
                    { 1, "Reason", null, "NewListing" },
                    { 2, "Reason", null, "Damaged" },
                    { 3, "Reason", null, "Depleted" },
                    { 4, "Reason", null, "Missing" },
                    { 5, "Reason", null, "Expired" },
                    { 0, "TrOrderBatchStatus", null, "All" },
                    { 1, "TrOrderBatchStatus", null, "Pending" },
                    { 2, "TrOrderBatchStatus", null, "Completed" },
                    { 3, "TrOrderBatchStatus", null, "Overdue" },
                    { 0, "TrOrderStatus", null, "All" },
                    { 1, "TrOrderStatus", null, "Pending" },
                    { 2, "TrOrderStatus", null, "Approved" },
                    { 3, "TrOrderStatus", null, "Rejected" },
                    { 4, "TrOrderStatus", null, "Processed" },
                    { 5, "TrOrderStatus", null, "Fulfilled" },
                    { 6, "TrOrderStatus", null, "Unfulfilled" },
                    { 7, "TrOrderStatus", null, "Cancelled" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__EnumLook__AB52D1D3A9AB9B15",
                table: "EnumLookUp");

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 1, "Brand" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 2, "Brand" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 0, "HhtOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 1, "HhtOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 2, "HhtOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 3, "HhtOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 4, "HhtOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 5, "HhtOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 6, "HhtOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 0, "Reason" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 1, "Reason" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 2, "Reason" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 3, "Reason" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 4, "Reason" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 5, "Reason" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 0, "TrOrderBatchStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 1, "TrOrderBatchStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 2, "TrOrderBatchStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 3, "TrOrderBatchStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 0, "TrOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 1, "TrOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 2, "TrOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 3, "TrOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 4, "TrOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 5, "TrOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 6, "TrOrderStatus" });

            migrationBuilder.DeleteData(
                table: "EnumLookUp",
                keyColumns: new[] { "EnumId", "EnumName" },
                keyValues: new object[] { 7, "TrOrderStatus" });

            migrationBuilder.CreateIndex(
                name: "UQ__EnumLook__AB52D1D28856552A",
                table: "EnumLookUp",
                columns: new[] { "EnumName", "EnumId" },
                unique: true);
        }
    }
}
