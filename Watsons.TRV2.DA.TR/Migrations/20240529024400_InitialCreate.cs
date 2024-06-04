using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watsons.TRV2.DA.TR.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnumLookUp",
                columns: table => new
                {
                    EnumName = table.Column<string>(type: "varchar(75)", unicode: false, maxLength: 75, nullable: false),
                    EnumValue = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EnumId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    Action = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Module__2B7477A7189EC7AB", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "SalesBand",
                columns: table => new
                {
                    SalesBandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SalesBand = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SalesBan__D823A91564AE6BF6", x => x.SalesBandId);
                });

            migrationBuilder.CreateTable(
                name: "SysParam",
                columns: table => new
                {
                    Param = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Param", x => x.Param);
                });

            migrationBuilder.CreateTable(
                name: "TrafficLog",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AccessToken = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    AbsoluteUrlWithQuery = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Action = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Headers = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HttpStatus = table.Column<int>(type: "int", nullable: true),
                    RequestDT = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ResponseDT = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeTaken = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrafficL__33A8517A4A641DD8", x => x.RequestId);
                });

            migrationBuilder.CreateTable(
                name: "TrCart",
                columns: table => new
                {
                    TrCartId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plu = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Barcode = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    Brand = table.Column<byte>(type: "tinyint", nullable: false),
                    BrandName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Reason = table.Column<byte>(type: "tinyint", nullable: true),
                    Justification = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SupplierCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrCart__6B0492B214083B72", x => x.TrCartId);
                });

            migrationBuilder.CreateTable(
                name: "TrOrderBatch",
                columns: table => new
                {
                    TrOrderBatchId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<byte>(type: "tinyint", nullable: false),
                    TrOrderBatchStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrOrderB__DC57238D5DF06F15", x => x.TrOrderBatchId);
                });

            migrationBuilder.CreateTable(
                name: "RoleModuleAccess",
                columns: table => new
                {
                    RoleModuleAccessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RoleModu__06D8C239C1D45645", x => x.RoleModuleAccessId);
                    table.ForeignKey(
                        name: "FK_RoleModuleAccess_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "ModuleId");
                });

            migrationBuilder.CreateTable(
                name: "StoreSalesBand",
                columns: table => new
                {
                    StoreSalesBandId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SalesBandId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StoreSal__19B5EBDFCADBEB46", x => x.StoreSalesBandId);
                    table.ForeignKey(
                        name: "FK_StoreBand_SalesBandId",
                        column: x => x.SalesBandId,
                        principalTable: "SalesBand",
                        principalColumn: "SalesBandId");
                });

            migrationBuilder.CreateTable(
                name: "OrderCost",
                columns: table => new
                {
                    OderCostId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrOrderBatchId = table.Column<long>(type: "bigint", nullable: false),
                    CostThresholdSnapshot = table.Column<decimal>(type: "decimal(19,6)", nullable: true),
                    AccumulatedCostApproved = table.Column<decimal>(type: "decimal(19,6)", nullable: true),
                    TotalOrderCost = table.Column<decimal>(type: "decimal(19,6)", nullable: true),
                    TotalCostApproved = table.Column<decimal>(type: "decimal(19,6)", nullable: true),
                    TotalCostRejected = table.Column<decimal>(type: "decimal(19,6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderCos__79F80376C325CB63", x => x.OderCostId);
                    table.ForeignKey(
                        name: "FK_OrderBatchId",
                        column: x => x.TrOrderBatchId,
                        principalTable: "TrOrderBatch",
                        principalColumn: "TrOrderBatchId");
                });

            migrationBuilder.CreateTable(
                name: "TrOrder",
                columns: table => new
                {
                    TrOrderId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrOrderBatchId = table.Column<long>(type: "bigint", nullable: false),
                    TrCartId = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    BrandName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Plu = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Barcode = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Reason = table.Column<byte>(type: "tinyint", nullable: true),
                    Justification = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    IsRequireJustify = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    TrOrderStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SupplierCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AverageCost = table.Column<decimal>(type: "decimal(14,8)", nullable: true),
                    PluCappedSnapshot = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastWriteOffAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrOrder__11569D3E3851B48F", x => x.TrOrderId);
                    table.ForeignKey(
                        name: "FK_TrOrder_TrCartId",
                        column: x => x.TrCartId,
                        principalTable: "TrCart",
                        principalColumn: "TrCartId");
                    table.ForeignKey(
                        name: "FK_TrOrder_TrOrderBatchId",
                        column: x => x.TrOrderBatchId,
                        principalTable: "TrOrderBatch",
                        principalColumn: "TrOrderBatchId");
                });

            migrationBuilder.CreateTable(
                name: "B2bOrder",
                columns: table => new
                {
                    TrOrderId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    B2bFileName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    HhtInsertStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    HhtInsertAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    HhtRemark = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ReceivedQty = table.Column<int>(type: "int", nullable: true),
                    StoreReceivedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__B2bOrder__11569D3EE16F0FF2", x => x.TrOrderId);
                    table.ForeignKey(
                        name: "FK_B2BOrder_TrOrderId",
                        column: x => x.TrOrderId,
                        principalTable: "TrOrder",
                        principalColumn: "TrOrderId");
                });

            migrationBuilder.CreateTable(
                name: "StoreAdjustment",
                columns: table => new
                {
                    StoreAdjustmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrOrderBatchId = table.Column<long>(type: "bigint", nullable: false),
                    TrOrderId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Plu = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    ReasonCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Remark = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    InventoryAdjustmentNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StoreAdj__7930F22D47410319", x => x.StoreAdjustmentId);
                    table.ForeignKey(
                        name: "FK_StoreAdjustment_TrOrderBatchId",
                        column: x => x.TrOrderBatchId,
                        principalTable: "TrOrderBatch",
                        principalColumn: "TrOrderBatchId");
                    table.ForeignKey(
                        name: "FK_StoreAdjustment_TrOrderId",
                        column: x => x.TrOrderId,
                        principalTable: "TrOrder",
                        principalColumn: "TrOrderId");
                });

            migrationBuilder.CreateTable(
                name: "TrImage",
                columns: table => new
                {
                    TrImageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrOrderId = table.Column<long>(type: "bigint", nullable: true),
                    TrCartId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrImage__D6BA17142CE76803", x => x.TrImageId);
                    table.ForeignKey(
                        name: "FK_TrCartId",
                        column: x => x.TrCartId,
                        principalTable: "TrCart",
                        principalColumn: "TrCartId");
                    table.ForeignKey(
                        name: "FK_TrOrderId",
                        column: x => x.TrOrderId,
                        principalTable: "TrOrder",
                        principalColumn: "TrOrderId");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__EnumLook__AB52D1D28856552A",
                table: "EnumLookUp",
                columns: new[] { "EnumName", "EnumId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__OrderCos__DC57238C6F31B7AD",
                table: "OrderCost",
                column: "TrOrderBatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleModuleAccess_ModuleId",
                table: "RoleModuleAccess",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "idx_unique_salesband",
                table: "SalesBand",
                columns: new[] { "Type", "SalesBand" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreAdjustment_TrOrderBatchId",
                table: "StoreAdjustment",
                column: "TrOrderBatchId");

            migrationBuilder.CreateIndex(
                name: "UNIQUE_TrOrderId",
                table: "StoreAdjustment",
                column: "TrOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreSalesBand_SalesBandId",
                table: "StoreSalesBand",
                column: "SalesBandId");

            migrationBuilder.CreateIndex(
                name: "UQ__StoreSal__84197B48C7F11141",
                table: "StoreSalesBand",
                columns: new[] { "StoreId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_store",
                table: "TrCart",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TrImage_TrCartId",
                table: "TrImage",
                column: "TrCartId");

            migrationBuilder.CreateIndex(
                name: "IX_TrImage_TrOrderId",
                table: "TrImage",
                column: "TrOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrOrder_TrCartId",
                table: "TrOrder",
                column: "TrCartId");

            migrationBuilder.CreateIndex(
                name: "IX_TrOrder_TrOrderBatchId",
                table: "TrOrder",
                column: "TrOrderBatchId");

            migrationBuilder.CreateIndex(
                name: "idx_store",
                table: "TrOrderBatch",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B2bOrder");

            migrationBuilder.DropTable(
                name: "EnumLookUp");

            migrationBuilder.DropTable(
                name: "OrderCost");

            migrationBuilder.DropTable(
                name: "RoleModuleAccess");

            migrationBuilder.DropTable(
                name: "StoreAdjustment");

            migrationBuilder.DropTable(
                name: "StoreSalesBand");

            migrationBuilder.DropTable(
                name: "SysParam");

            migrationBuilder.DropTable(
                name: "TrafficLog");

            migrationBuilder.DropTable(
                name: "TrImage");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "SalesBand");

            migrationBuilder.DropTable(
                name: "TrOrder");

            migrationBuilder.DropTable(
                name: "TrCart");

            migrationBuilder.DropTable(
                name: "TrOrderBatch");
        }
    }
}
