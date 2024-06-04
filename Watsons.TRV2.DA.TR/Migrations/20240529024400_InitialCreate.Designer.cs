﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Watsons.TRV2.DA.TR.Entities;

#nullable disable

namespace Watsons.TRV2.DA.TR.Migrations
{
    [DbContext(typeof(TrContext))]
    [Migration("20240529024400_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.B2bOrder", b =>
                {
                    b.Property<long>("TrOrderId")
                        .HasColumnType("bigint");

                    b.Property<string>("B2bFileName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("HhtInsertAt")
                        .HasColumnType("datetime");

                    b.Property<byte?>("HhtInsertStatus")
                        .HasColumnType("tinyint");

                    b.Property<string>("HhtRemark")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("OrderNumber")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("ReceivedQty")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StoreReceivedAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.HasKey("TrOrderId")
                        .HasName("PK__B2bOrder__11569D3EE16F0FF2");

                    b.ToTable("B2bOrder", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.EnumLookUp", b =>
                {
                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("EnumId")
                        .HasColumnType("int");

                    b.Property<string>("EnumName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .IsUnicode(false)
                        .HasColumnType("varchar(75)");

                    b.Property<string>("EnumValue")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<byte?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.HasIndex(new[] { "EnumName", "EnumId" }, "UQ__EnumLook__AB52D1D28856552A")
                        .IsUnique();

                    b.ToTable("EnumLookUp", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.Module", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ModuleId"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("varchar(3)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)");

                    b.Property<byte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.HasKey("ModuleId")
                        .HasName("PK__Module__2B7477A7189EC7AB");

                    b.ToTable("Module", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.OrderCost", b =>
                {
                    b.Property<long>("OderCostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("OderCostId"));

                    b.Property<decimal?>("AccumulatedCostApproved")
                        .HasColumnType("decimal(19, 6)");

                    b.Property<decimal?>("CostThresholdSnapshot")
                        .HasColumnType("decimal(19, 6)");

                    b.Property<decimal?>("TotalCostApproved")
                        .HasColumnType("decimal(19, 6)");

                    b.Property<decimal?>("TotalCostRejected")
                        .HasColumnType("decimal(19, 6)");

                    b.Property<decimal?>("TotalOrderCost")
                        .HasColumnType("decimal(19, 6)");

                    b.Property<long>("TrOrderBatchId")
                        .HasColumnType("bigint");

                    b.HasKey("OderCostId")
                        .HasName("PK__OrderCos__79F80376C325CB63");

                    b.HasIndex(new[] { "TrOrderBatchId" }, "UQ__OrderCos__DC57238C6F31B7AD")
                        .IsUnique();

                    b.ToTable("OrderCost", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.RoleModuleAccess", b =>
                {
                    b.Property<int>("RoleModuleAccessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleModuleAccessId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("ModuleId")
                        .HasColumnType("int");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.HasKey("RoleModuleAccessId")
                        .HasName("PK__RoleModu__06D8C239C1D45645");

                    b.HasIndex("ModuleId");

                    b.ToTable("RoleModuleAccess", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.SalesBand", b =>
                {
                    b.Property<int>("SalesBandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SalesBandId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SalesBand1")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("SalesBand");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(12, 4)");

                    b.HasKey("SalesBandId")
                        .HasName("PK__SalesBan__D823A91564AE6BF6");

                    b.HasIndex(new[] { "Type", "SalesBand1" }, "idx_unique_salesband")
                        .IsUnique();

                    b.ToTable("SalesBand", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.StoreAdjustment", b =>
                {
                    b.Property<long>("StoreAdjustmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("StoreAdjustmentId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("InventoryAdjustmentNumber")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Plu")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Qty")
                        .HasColumnType("int");

                    b.Property<string>("ReasonCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Remark")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<long>("TrOrderBatchId")
                        .HasColumnType("bigint");

                    b.Property<long>("TrOrderId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.HasKey("StoreAdjustmentId")
                        .HasName("PK__StoreAdj__7930F22D47410319");

                    b.HasIndex("TrOrderBatchId");

                    b.HasIndex(new[] { "TrOrderId" }, "UNIQUE_TrOrderId")
                        .IsUnique();

                    b.ToTable("StoreAdjustment", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.StoreSalesBand", b =>
                {
                    b.Property<long>("StoreSalesBandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("StoreSalesBandId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("SalesBandId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("StoreSalesBandId")
                        .HasName("PK__StoreSal__19B5EBDFCADBEB46");

                    b.HasIndex("SalesBandId");

                    b.HasIndex(new[] { "StoreId", "Type" }, "UQ__StoreSal__84197B48C7F11141")
                        .IsUnique();

                    b.ToTable("StoreSalesBand", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.SysParam", b =>
                {
                    b.Property<string>("Param")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Value")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Param")
                        .HasName("PK_Param");

                    b.ToTable("SysParam", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrCart", b =>
                {
                    b.Property<long>("TrCartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TrCartId"));

                    b.Property<string>("Barcode")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<byte>("Brand")
                        .HasColumnType("tinyint");

                    b.Property<string>("BrandName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Justification")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Plu")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("ProductName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<byte?>("Reason")
                        .HasColumnType("tinyint");

                    b.Property<int?>("StoreId")
                        .HasColumnType("int");

                    b.Property<string>("SupplierCode")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SupplierName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("TrCartId")
                        .HasName("PK__TrCart__6B0492B214083B72");

                    b.HasIndex(new[] { "StoreId" }, "idx_store");

                    b.ToTable("TrCart", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrImage", b =>
                {
                    b.Property<long>("TrImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TrImageId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("TrCartId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TrOrderId")
                        .HasColumnType("bigint");

                    b.HasKey("TrImageId")
                        .HasName("PK__TrImage__D6BA17142CE76803");

                    b.HasIndex("TrCartId");

                    b.HasIndex("TrOrderId");

                    b.ToTable("TrImage", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrOrder", b =>
                {
                    b.Property<long>("TrOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TrOrderId"));

                    b.Property<decimal?>("AverageCost")
                        .HasColumnType("decimal(14, 8)");

                    b.Property<string>("Barcode")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("BrandName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool?>("IsRequireJustify")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Justification")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("LastWriteOffAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Plu")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<decimal?>("PluCappedSnapshot")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("ProductName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<byte?>("Reason")
                        .HasColumnType("tinyint");

                    b.Property<string>("Remark")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("SupplierCode")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SupplierName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("TrCartId")
                        .HasColumnType("bigint");

                    b.Property<long>("TrOrderBatchId")
                        .HasColumnType("bigint");

                    b.Property<byte?>("TrOrderStatus")
                        .HasColumnType("tinyint");

                    b.HasKey("TrOrderId")
                        .HasName("PK__TrOrder__11569D3E3851B48F");

                    b.HasIndex("TrCartId");

                    b.HasIndex("TrOrderBatchId");

                    b.ToTable("TrOrder", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrOrderBatch", b =>
                {
                    b.Property<long>("TrOrderBatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TrOrderBatchId"));

                    b.Property<byte>("Brand")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<byte>("TrOrderBatchStatus")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("TrOrderBatchId")
                        .HasName("PK__TrOrderB__DC57238D5DF06F15");

                    b.HasIndex(new[] { "StoreId" }, "idx_store");

                    b.ToTable("TrOrderBatch", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrafficLog", b =>
                {
                    b.Property<Guid>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("AbsoluteUrlWithQuery")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("AccessToken")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Action")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Headers")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<int?>("HttpStatus")
                        .HasColumnType("int");

                    b.Property<string>("Request")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RequestDt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("RequestDT")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResponseDt")
                        .HasColumnType("datetime")
                        .HasColumnName("ResponseDT");

                    b.Property<float?>("TimeTaken")
                        .HasColumnType("real");

                    b.HasKey("RequestId")
                        .HasName("PK__TrafficL__33A8517A4A641DD8");

                    b.ToTable("TrafficLog", (string)null);
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.B2bOrder", b =>
                {
                    b.HasOne("Watsons.TRV2.DA.TR.Entities.TrOrder", "TrOrder")
                        .WithOne("B2bOrder")
                        .HasForeignKey("Watsons.TRV2.DA.TR.Entities.B2bOrder", "TrOrderId")
                        .IsRequired()
                        .HasConstraintName("FK_B2BOrder_TrOrderId");

                    b.Navigation("TrOrder");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.OrderCost", b =>
                {
                    b.HasOne("Watsons.TRV2.DA.TR.Entities.TrOrderBatch", "TrOrderBatch")
                        .WithOne("OrderCost")
                        .HasForeignKey("Watsons.TRV2.DA.TR.Entities.OrderCost", "TrOrderBatchId")
                        .IsRequired()
                        .HasConstraintName("FK_OrderBatchId");

                    b.Navigation("TrOrderBatch");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.RoleModuleAccess", b =>
                {
                    b.HasOne("Watsons.TRV2.DA.TR.Entities.Module", "Module")
                        .WithMany("RoleModuleAccesses")
                        .HasForeignKey("ModuleId")
                        .IsRequired()
                        .HasConstraintName("FK_RoleModuleAccess_ModuleId");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.StoreAdjustment", b =>
                {
                    b.HasOne("Watsons.TRV2.DA.TR.Entities.TrOrderBatch", "TrOrderBatch")
                        .WithMany("StoreAdjustments")
                        .HasForeignKey("TrOrderBatchId")
                        .IsRequired()
                        .HasConstraintName("FK_StoreAdjustment_TrOrderBatchId");

                    b.HasOne("Watsons.TRV2.DA.TR.Entities.TrOrder", "TrOrder")
                        .WithOne("StoreAdjustment")
                        .HasForeignKey("Watsons.TRV2.DA.TR.Entities.StoreAdjustment", "TrOrderId")
                        .IsRequired()
                        .HasConstraintName("FK_StoreAdjustment_TrOrderId");

                    b.Navigation("TrOrder");

                    b.Navigation("TrOrderBatch");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.StoreSalesBand", b =>
                {
                    b.HasOne("Watsons.TRV2.DA.TR.Entities.SalesBand", "SalesBand")
                        .WithMany("StoreSalesBands")
                        .HasForeignKey("SalesBandId")
                        .IsRequired()
                        .HasConstraintName("FK_StoreBand_SalesBandId");

                    b.Navigation("SalesBand");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrImage", b =>
                {
                    b.HasOne("Watsons.TRV2.DA.TR.Entities.TrCart", "TrCart")
                        .WithMany("TrImages")
                        .HasForeignKey("TrCartId")
                        .HasConstraintName("FK_TrCartId");

                    b.HasOne("Watsons.TRV2.DA.TR.Entities.TrOrder", "TrOrder")
                        .WithMany("TrImages")
                        .HasForeignKey("TrOrderId")
                        .HasConstraintName("FK_TrOrderId");

                    b.Navigation("TrCart");

                    b.Navigation("TrOrder");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrOrder", b =>
                {
                    b.HasOne("Watsons.TRV2.DA.TR.Entities.TrCart", "TrCart")
                        .WithMany("TrOrders")
                        .HasForeignKey("TrCartId")
                        .IsRequired()
                        .HasConstraintName("FK_TrOrder_TrCartId");

                    b.HasOne("Watsons.TRV2.DA.TR.Entities.TrOrderBatch", "TrOrderBatch")
                        .WithMany("TrOrders")
                        .HasForeignKey("TrOrderBatchId")
                        .IsRequired()
                        .HasConstraintName("FK_TrOrder_TrOrderBatchId");

                    b.Navigation("TrCart");

                    b.Navigation("TrOrderBatch");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.Module", b =>
                {
                    b.Navigation("RoleModuleAccesses");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.SalesBand", b =>
                {
                    b.Navigation("StoreSalesBands");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrCart", b =>
                {
                    b.Navigation("TrImages");

                    b.Navigation("TrOrders");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrOrder", b =>
                {
                    b.Navigation("B2bOrder");

                    b.Navigation("StoreAdjustment");

                    b.Navigation("TrImages");
                });

            modelBuilder.Entity("Watsons.TRV2.DA.TR.Entities.TrOrderBatch", b =>
                {
                    b.Navigation("OrderCost");

                    b.Navigation("StoreAdjustments");

                    b.Navigation("TrOrders");
                });
#pragma warning restore 612, 618
        }
    }
}
