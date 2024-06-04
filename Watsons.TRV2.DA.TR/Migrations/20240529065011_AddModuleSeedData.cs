using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Watsons.TRV2.DA.TR.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Module",
                columns: new[] { "ModuleId", "Action", "CreatedBy", "ModuleName", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "R", "System", "ORDER_OWN", (byte)1, null, null },
                    { 2, "W", "System", "ORDER_OWN", (byte)1, null, null },
                    { 3, "R", "System", "ORDER_SUPPLIER", (byte)1, null, null },
                    { 4, "R", "System", "REPORT_OWN", (byte)1, null, null },
                    { 5, "E", "System", "REPORT_OWN", (byte)1, null, null },
                    { 6, "R", "System", "REPORT_SUPPLIER", (byte)1, null, null },
                    { 7, "E", "System", "REPORT_SUPPLIER", (byte)1, null, null },
                    { 8, "R", "System", "REPORT_FULFILLMENT", (byte)1, null, null },
                    { 9, "E", "System", "REPORT_FULFILLMENT", (byte)1, null, null }
                });

            migrationBuilder.InsertData(
                table: "RoleModuleAccess",
                columns: new[] { "RoleModuleAccessId", "CreatedBy", "ModuleId", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "System", 1, new Guid("37e1bfd1-45b7-4f19-8c6c-ac6d6bf12ca0"), null, null },
                    { 2, "System", 3, new Guid("37e1bfd1-45b7-4f19-8c6c-ac6d6bf12ca0"), null, null },
                    { 3, "System", 4, new Guid("37e1bfd1-45b7-4f19-8c6c-ac6d6bf12ca0"), null, null },
                    { 4, "System", 5, new Guid("37e1bfd1-45b7-4f19-8c6c-ac6d6bf12ca0"), null, null },
                    { 5, "System", 6, new Guid("37e1bfd1-45b7-4f19-8c6c-ac6d6bf12ca0"), null, null },
                    { 6, "System", 7, new Guid("37e1bfd1-45b7-4f19-8c6c-ac6d6bf12ca0"), null, null },
                    { 7, "System", 8, new Guid("37e1bfd1-45b7-4f19-8c6c-ac6d6bf12ca0"), null, null },
                    { 8, "System", 9, new Guid("37e1bfd1-45b7-4f19-8c6c-ac6d6bf12ca0"), null, null },
                    { 9, "System", 1, new Guid("7f195f8e-2fce-4404-a8db-097f212cc2bf"), null, null },
                    { 10, "System", 3, new Guid("7f195f8e-2fce-4404-a8db-097f212cc2bf"), null, null },
                    { 11, "System", 4, new Guid("7f195f8e-2fce-4404-a8db-097f212cc2bf"), null, null },
                    { 12, "System", 5, new Guid("7f195f8e-2fce-4404-a8db-097f212cc2bf"), null, null },
                    { 13, "System", 6, new Guid("7f195f8e-2fce-4404-a8db-097f212cc2bf"), null, null },
                    { 14, "System", 7, new Guid("7f195f8e-2fce-4404-a8db-097f212cc2bf"), null, null },
                    { 15, "System", 8, new Guid("7f195f8e-2fce-4404-a8db-097f212cc2bf"), null, null },
                    { 16, "System", 9, new Guid("7f195f8e-2fce-4404-a8db-097f212cc2bf"), null, null },
                    { 17, "System", 1, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 18, "System", 2, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 19, "System", 3, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 20, "System", 4, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 21, "System", 5, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 22, "System", 6, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 23, "System", 7, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 24, "System", 8, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 25, "System", 9, new Guid("41560460-c203-4212-a341-6860008ad007"), null, null },
                    { 26, "System", 1, new Guid("8d39cca7-edf9-49ed-a959-3d0057b055c2"), null, null },
                    { 27, "System", 2, new Guid("8d39cca7-edf9-49ed-a959-3d0057b055c2"), null, null },
                    { 28, "System", 3, new Guid("8d39cca7-edf9-49ed-a959-3d0057b055c2"), null, null },
                    { 29, "System", 1, new Guid("bac83bf7-9c2c-46ee-b79f-d64094fff01e"), null, null },
                    { 30, "System", 2, new Guid("bac83bf7-9c2c-46ee-b79f-d64094fff01e"), null, null },
                    { 31, "System", 3, new Guid("bac83bf7-9c2c-46ee-b79f-d64094fff01e"), null, null },
                    { 32, "System", 1, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null },
                    { 33, "System", 2, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null },
                    { 34, "System", 3, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null },
                    { 35, "System", 4, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null },
                    { 36, "System", 5, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null },
                    { 37, "System", 6, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null },
                    { 38, "System", 7, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null },
                    { 39, "System", 8, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null },
                    { 40, "System", 9, new Guid("1b6f6c6d-53b8-4613-a57e-079a3084aaee"), null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "RoleModuleAccess",
                keyColumn: "RoleModuleAccessId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "ModuleId",
                keyValue: 9);
        }
    }
}
