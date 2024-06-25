using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrContext : DbContext
{
    public TrContext()
    {
    }

    public TrContext(DbContextOptions<TrContext> options)
        : base(options)
    {
    }

    internal virtual DbSet<B2bOrder> B2bOrders { get; set; }

    internal virtual DbSet<EnumLookUp> EnumLookUps { get; set; }

    internal virtual DbSet<Module> Modules { get; set; }

    internal virtual DbSet<OrderCost> OrderCosts { get; set; }

    internal virtual DbSet<RoleModuleAccess> RoleModuleAccesses { get; set; }

    internal virtual DbSet<SalesBand> SalesBands { get; set; }

    internal virtual DbSet<StoreAdjustment> StoreAdjustments { get; set; }

    internal virtual DbSet<StoreSalesBand> StoreSalesBands { get; set; }

    internal virtual DbSet<SysParam> SysParams { get; set; }

    internal virtual DbSet<TrCart> TrCarts { get; set; }

    internal virtual DbSet<TrImage> TrImages { get; set; }

    internal virtual DbSet<TrOrder> TrOrders { get; set; }

    internal virtual DbSet<TrOrderBatch> TrOrderBatches { get; set; }

    internal virtual DbSet<TrafficLog> TrafficLogs { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=10.98.32.248;Database=TRV2;User ID=sa;Password=!QAZ2wsx#EDC;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<B2bOrder>(entity =>
        {
            entity.HasKey(e => e.TrOrderId).HasName("PK__B2bOrder__11569D3EE16F0FF2");

            entity.ToTable("B2bOrder");

            entity.Property(e => e.TrOrderId).ValueGeneratedNever();
            entity.Property(e => e.B2bFileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HhtInsertAt).HasColumnType("datetime");
            entity.Property(e => e.HhtRemark)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StoreReceivedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.TrOrder).WithOne(p => p.B2bOrder)
                .HasForeignKey<B2bOrder>(d => d.TrOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_B2BOrder_TrOrderId");
        });

        modelBuilder.Entity<EnumLookUp>(entity =>
        {
            entity.HasKey(e => new { e.EnumName, e.EnumId }).HasName("PK__EnumLook__AB52D1D3A9AB9B15");

            entity.ToTable("EnumLookUp");

            entity.Property(e => e.EnumName)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EnumValue)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);

            entity.HasData(
                new EnumLookUp { EnumName = "TrOrderBatchStatus", EnumValue = "All", EnumId = 0 },
                new EnumLookUp { EnumName = "TrOrderBatchStatus", EnumValue = "Pending", EnumId = 1 },
                new EnumLookUp { EnumName = "TrOrderBatchStatus", EnumValue = "Completed", EnumId = 2 },
                new EnumLookUp { EnumName = "TrOrderBatchStatus", EnumValue = "Overdue", EnumId = 3 },
                new EnumLookUp { EnumName = "TrOrderStatus", EnumValue = "All", EnumId = 0 },
                new EnumLookUp { EnumName = "TrOrderStatus", EnumValue = "Pending", EnumId = 1 },
                new EnumLookUp { EnumName = "TrOrderStatus", EnumValue = "Approved", EnumId = 2 },
                new EnumLookUp { EnumName = "TrOrderStatus", EnumValue = "Rejected", EnumId = 3 },
                new EnumLookUp { EnumName = "TrOrderStatus", EnumValue = "Processed", EnumId = 4 },
                new EnumLookUp { EnumName = "TrOrderStatus", EnumValue = "Fulfilled", EnumId = 5 },
                new EnumLookUp { EnumName = "TrOrderStatus", EnumValue = "Unfulfilled", EnumId = 6 },
                new EnumLookUp { EnumName = "TrOrderStatus", EnumValue = "Cancelled", EnumId = 7 },
                new EnumLookUp { EnumName = "HhtOrderStatus", EnumValue = "Error", EnumId = 0 },
                new EnumLookUp { EnumName = "HhtOrderStatus", EnumValue = "Pending", EnumId = 1 },
                new EnumLookUp { EnumName = "HhtOrderStatus", EnumValue = "Shipping", EnumId = 2 },
                new EnumLookUp { EnumName = "HhtOrderStatus", EnumValue = "Shipped", EnumId = 3 },
                new EnumLookUp { EnumName = "HhtOrderStatus", EnumValue = "Cancelled", EnumId = 4 },
                new EnumLookUp { EnumName = "HhtOrderStatus", EnumValue = "Expire", EnumId = 5 },
                new EnumLookUp { EnumName = "HhtOrderStatus", EnumValue = "StoreReceived", EnumId = 6 },
                new EnumLookUp { EnumName = "Brand", EnumValue = "Own", EnumId = 1 },
                new EnumLookUp { EnumName = "Brand", EnumValue = "Supplier", EnumId = 2 },
                new EnumLookUp { EnumName = "Reason", EnumValue = "None", EnumId = 0 },
                new EnumLookUp { EnumName = "Reason", EnumValue = "NewListing", EnumId = 1 },
                new EnumLookUp { EnumName = "Reason", EnumValue = "Damaged", EnumId = 2 },
                new EnumLookUp { EnumName = "Reason", EnumValue = "Depleted", EnumId = 3 },
                new EnumLookUp { EnumName = "Reason", EnumValue = "Missing", EnumId = 4 },
                new EnumLookUp { EnumName = "Reason", EnumValue = "Expired", EnumId = 5 }
            );
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Module__2B7477A7189EC7AB");

            entity.ToTable("Module");

            entity.Property(e => e.Action)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModuleName)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasData(
                new Module { ModuleId = 1, ModuleName = "ORDER_OWN", Action = "R", Status = 1, CreatedBy = "System" },
                new Module { ModuleId = 2, ModuleName = "ORDER_OWN", Action = "W", Status = 1, CreatedBy = "System" },
                new Module { ModuleId = 3, ModuleName = "ORDER_SUPPLIER", Action = "R", Status = 1, CreatedBy = "System" },
                new Module { ModuleId = 4, ModuleName = "REPORT_OWN", Action = "R", Status = 1, CreatedBy = "System" },
                new Module { ModuleId = 5, ModuleName = "REPORT_OWN", Action = "E", Status = 1, CreatedBy = "System" },
                new Module { ModuleId = 6, ModuleName = "REPORT_SUPPLIER", Action = "R", Status = 1, CreatedBy = "System" },
                new Module { ModuleId = 7, ModuleName = "REPORT_SUPPLIER", Action = "E", Status = 1, CreatedBy = "System" },
                new Module { ModuleId = 8, ModuleName = "REPORT_FULFILLMENT", Action = "R", Status = 1, CreatedBy = "System" },
                new Module { ModuleId = 9, ModuleName = "REPORT_FULFILLMENT", Action = "E", Status = 1, CreatedBy = "System" }
            );
        });

        modelBuilder.Entity<OrderCost>(entity =>
        {
            entity.HasKey(e => e.OderCostId).HasName("PK__OrderCos__79F80376C325CB63");

            entity.ToTable("OrderCost");

            entity.HasIndex(e => e.TrOrderBatchId, "UQ__OrderCos__DC57238C6F31B7AD").IsUnique();

            entity.Property(e => e.AccumulatedCostApproved).HasColumnType("decimal(19, 6)");
            entity.Property(e => e.CostThresholdSnapshot).HasColumnType("decimal(19, 6)");
            entity.Property(e => e.TotalCostApproved).HasColumnType("decimal(19, 6)");
            entity.Property(e => e.TotalCostRejected).HasColumnType("decimal(19, 6)");
            entity.Property(e => e.TotalOrderCost).HasColumnType("decimal(19, 6)");

            entity.HasOne(d => d.TrOrderBatch).WithOne(p => p.OrderCost)
                .HasForeignKey<OrderCost>(d => d.TrOrderBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderBatchId");
        });

        modelBuilder.Entity<RoleModuleAccess>(entity =>
        {
            entity.HasKey(e => e.RoleModuleAccessId).HasName("PK__RoleModu__06D8C239C1D45645");

            entity.ToTable("RoleModuleAccess");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Module).WithMany(p => p.RoleModuleAccesses)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModuleAccess_ModuleId");

            entity.HasData(
                new RoleModuleAccess { RoleModuleAccessId = 1, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 1, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 2, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 2, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 3, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 3, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 4, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 4, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 5, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 5, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 6, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 6, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 7, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 7, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 8, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 8, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 9, RoleId = new Guid("37E1BFD1-45B7-4F19-8C6C-AC6D6BF12CA0"), ModuleId = 9, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 10, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 1, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 11, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 2, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 12, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 3, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 13, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 4, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 14, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 5, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 15, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 6, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 16, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 7, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 17, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 8, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 18, RoleId = new Guid("7F195F8E-2FCE-4404-A8DB-097F212CC2BF"), ModuleId = 9, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 19, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 1, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 20, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 2, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 21, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 3, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 22, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 4, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 23, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 5, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 24, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 6, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 25, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 7, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 26, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 8, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 27, RoleId = new Guid("41560460-C203-4212-A341-6860008AD007"), ModuleId = 9, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 28, RoleId = new Guid("8D39CCA7-EDF9-49ED-A959-3D0057B055C2"), ModuleId = 1, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 29, RoleId = new Guid("8D39CCA7-EDF9-49ED-A959-3D0057B055C2"), ModuleId = 2, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 30, RoleId = new Guid("8D39CCA7-EDF9-49ED-A959-3D0057B055C2"), ModuleId = 3, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 31, RoleId = new Guid("BAC83BF7-9C2C-46EE-B79F-D64094FFF01E"), ModuleId = 1, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 32, RoleId = new Guid("BAC83BF7-9C2C-46EE-B79F-D64094FFF01E"), ModuleId = 2, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 33, RoleId = new Guid("BAC83BF7-9C2C-46EE-B79F-D64094FFF01E"), ModuleId = 3, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 34, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 1, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 35, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 2, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 36, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 3, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 37, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 4, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 38, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 5, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 39, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 6, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 40, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 7, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 41, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 8, CreatedBy = "System" },
                new RoleModuleAccess { RoleModuleAccessId = 42, RoleId = new Guid("1B6F6C6D-53B8-4613-A57E-079A3084AAEE"), ModuleId = 9, CreatedBy = "System" }
            );
        });

        modelBuilder.Entity<SalesBand>(entity =>
        {
            entity.HasKey(e => e.SalesBandId).HasName("PK__SalesBan__D823A91564AE6BF6");

            entity.ToTable("SalesBand");

            entity.HasIndex(e => new { e.Type, e.SalesBand1 }, "idx_unique_salesband").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SalesBand1)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SalesBand");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnType("decimal(12, 4)");

            entity.HasData(
               new SalesBand { SalesBandId = 1, Type = "PLU_UNIT_LIMIT_OWN", SalesBand1 = "A", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 2, Type = "PLU_UNIT_LIMIT_OWN", SalesBand1 = "B", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 3, Type = "PLU_UNIT_LIMIT_OWN", SalesBand1 = "C", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 4, Type = "PLU_UNIT_LIMIT_OWN", SalesBand1 = "D", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 5, Type = "PLU_UNIT_LIMIT_OWN", SalesBand1 = "E", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 6, Type = "PLU_UNIT_LIMIT_OWN", SalesBand1 = "F", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 7, Type = "COST_LIMIT_OWN", SalesBand1 = "A", Value = 650, CreatedBy = "System" },
               new SalesBand { SalesBandId = 8, Type = "COST_LIMIT_OWN", SalesBand1 = "B", Value = 650, CreatedBy = "System" },
               new SalesBand { SalesBandId = 9, Type = "COST_LIMIT_OWN", SalesBand1 = "C", Value = 650, CreatedBy = "System" },
               new SalesBand { SalesBandId = 10, Type = "COST_LIMIT_OWN", SalesBand1 = "D", Value = 550, CreatedBy = "System" },
               new SalesBand { SalesBandId = 11, Type = "COST_LIMIT_OWN", SalesBand1 = "E", Value = 550, CreatedBy = "System" },
               new SalesBand { SalesBandId = 12, Type = "COST_LIMIT_OWN", SalesBand1 = "F", Value = 550, CreatedBy = "System" },
               new SalesBand { SalesBandId = 13, Type = "PLU_UNIT_LIMIT_SUPPLIER", SalesBand1 = "A", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 14, Type = "PLU_UNIT_LIMIT_SUPPLIER", SalesBand1 = "B", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 15, Type = "PLU_UNIT_LIMIT_SUPPLIER", SalesBand1 = "C", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 16, Type = "PLU_UNIT_LIMIT_SUPPLIER", SalesBand1 = "D", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 17, Type = "PLU_UNIT_LIMIT_SUPPLIER", SalesBand1 = "E", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 18, Type = "PLU_UNIT_LIMIT_SUPPLIER", SalesBand1 = "F", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 19, Type = "PLU_UNIT_LIMIT_COSMETIC", SalesBand1 = "A", Value = 2, CreatedBy = "System" },
               new SalesBand { SalesBandId = 20, Type = "PLU_UNIT_LIMIT_COSMETIC", SalesBand1 = "B", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 21, Type = "PLU_UNIT_LIMIT_COSMETIC", SalesBand1 = "C", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 22, Type = "PLU_UNIT_LIMIT_COSMETIC", SalesBand1 = "D", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 23, Type = "PLU_UNIT_LIMIT_COSMETIC", SalesBand1 = "E", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 24, Type = "PLU_UNIT_LIMIT_COSMETIC", SalesBand1 = "F", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 25, Type = "PLU_UNIT_LIMIT_SKINCARE", SalesBand1 = "A", Value = 2, CreatedBy = "System" },
               new SalesBand { SalesBandId = 26, Type = "PLU_UNIT_LIMIT_SKINCARE", SalesBand1 = "B", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 27, Type = "PLU_UNIT_LIMIT_SKINCARE", SalesBand1 = "C", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 28, Type = "PLU_UNIT_LIMIT_SKINCARE", SalesBand1 = "D", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 29, Type = "PLU_UNIT_LIMIT_SKINCARE", SalesBand1 = "E", Value = 1, CreatedBy = "System" },
               new SalesBand { SalesBandId = 30, Type = "PLU_UNIT_LIMIT_SKINCARE", SalesBand1 = "F", Value = 1, CreatedBy = "System" }
           );
        });

        modelBuilder.Entity<StoreAdjustment>(entity =>
        {
            entity.HasKey(e => e.StoreAdjustmentId).HasName("PK__StoreAdj__7930F22D47410319");

            entity.ToTable("StoreAdjustment");

            entity.HasIndex(e => e.TrOrderId, "UNIQUE_TrOrderId").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.InventoryAdjustmentNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Plu)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReasonCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.TrOrderBatch).WithMany(p => p.StoreAdjustments)
                .HasForeignKey(d => d.TrOrderBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreAdjustment_TrOrderBatchId");

            entity.HasOne(d => d.TrOrder).WithOne(p => p.StoreAdjustment)
                .HasForeignKey<StoreAdjustment>(d => d.TrOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreAdjustment_TrOrderId");
        });

        modelBuilder.Entity<StoreSalesBand>(entity =>
        {
            entity.HasKey(e => e.StoreSalesBandId).HasName("PK__StoreSal__19B5EBDFCADBEB46");

            entity.ToTable("StoreSalesBand");

            entity.HasIndex(e => new { e.StoreId, e.Type }, "UQ__StoreSal__84197B48C7F11141").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.SalesBand).WithMany(p => p.StoreSalesBands)
                .HasForeignKey(d => d.SalesBandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreBand_SalesBandId");
        });

        modelBuilder.Entity<SysParam>(entity =>
        {
            entity.HasKey(e => e.Param).HasName("PK_Param");

            entity.ToTable("SysParam");

            entity.Property(e => e.Param)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasData(
                new SysParam { Param = "OrderNumber", Value = "1000" }
            );
        });

        modelBuilder.Entity<TrCart>(entity =>
            {
                entity.HasKey(e => e.TrCartId).HasName("PK__TrCart__6B0492B214083B72");

                entity.ToTable("TrCart");

                entity.HasIndex(e => e.StoreId, "idx_store");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Justification)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Plu)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.SupplierCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SupplierName).HasMaxLength(200);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

        modelBuilder.Entity<TrImage>(entity =>
        {
            entity.HasKey(e => e.TrImageId).HasName("PK__TrImage__D6BA17142CE76803");

            entity.ToTable("TrImage");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImagePath).HasMaxLength(300);

            entity.HasOne(d => d.TrCart).WithMany(p => p.TrImages)
                .HasForeignKey(d => d.TrCartId)
                .HasConstraintName("FK_TrCartId");

            entity.HasOne(d => d.TrOrder).WithMany(p => p.TrImages)
                .HasForeignKey(d => d.TrOrderId)
                .HasConstraintName("FK_TrOrderId");
        });

        modelBuilder.Entity<TrOrder>(entity =>
        {
            entity.HasKey(e => e.TrOrderId).HasName("PK__TrOrder__11569D3E3851B48F");

            entity.ToTable("TrOrder");

            entity.Property(e => e.AverageCost).HasColumnType("decimal(14, 8)");
            entity.Property(e => e.Barcode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BrandName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsRequireJustify).HasDefaultValue(true);
            entity.Property(e => e.Justification)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastWriteOffAt).HasColumnType("datetime");
            entity.Property(e => e.Plu)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PluCappedSnapshot).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(200);

            entity.HasOne(d => d.TrCart).WithMany(p => p.TrOrders)
                .HasForeignKey(d => d.TrCartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrOrder_TrCartId");

            entity.HasOne(d => d.TrOrderBatch).WithMany(p => p.TrOrders)
                .HasForeignKey(d => d.TrOrderBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrOrder_TrOrderBatchId");
        });

        modelBuilder.Entity<TrOrderBatch>(entity =>
        {
            entity.HasKey(e => e.TrOrderBatchId).HasName("PK__TrOrderB__DC57238D5DF06F15");

            entity.ToTable("TrOrderBatch");

            entity.HasIndex(e => e.StoreId, "idx_store");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TrafficLog>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__TrafficL__33A8517A4A641DD8");

            entity.ToTable("TrafficLog");

            entity.Property(e => e.RequestId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AbsoluteUrlWithQuery).IsUnicode(false);
            entity.Property(e => e.AccessToken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Action)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Headers).IsUnicode(false);
            entity.Property(e => e.RequestDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("RequestDT");
            entity.Property(e => e.ResponseDt)
                .HasColumnType("datetime")
                .HasColumnName("ResponseDT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
