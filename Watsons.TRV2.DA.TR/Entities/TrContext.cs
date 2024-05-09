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

    public virtual DbSet<EnumLookUp> EnumLookUps { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<OrderCost> OrderCosts { get; set; }

    public virtual DbSet<RoleModuleAccess> RoleModuleAccesses { get; set; }

    public virtual DbSet<SalesBand> SalesBands { get; set; }

    public virtual DbSet<StoreAdjustment> StoreAdjustments { get; set; }

    public virtual DbSet<StoreSalesBand> StoreSalesBands { get; set; }

    public virtual DbSet<TrCart> TrCarts { get; set; }

    public virtual DbSet<TrImage> TrImages { get; set; }

    public virtual DbSet<TrOrder> TrOrders { get; set; }

    public virtual DbSet<TrOrderBatch> TrOrderBatches { get; set; }

    public virtual DbSet<TrafficLog> TrafficLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.98.32.248;Database=TRV2_UAT;User ID=sa;Password=!QAZ2wsx#EDC;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnumLookUp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EnumLookUp");

            entity.HasIndex(e => new { e.EnumName, e.EnumId }, "UQ__EnumLook__AB52D1D28856552A").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EnumName)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.EnumValue)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
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
