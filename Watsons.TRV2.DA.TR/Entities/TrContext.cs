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

    public virtual DbSet<SalesBand> SalesBands { get; set; }

    public virtual DbSet<StoreSalesBand> StoreSalesBands { get; set; }

    public virtual DbSet<TrCart> TrCarts { get; set; }

    public virtual DbSet<TrImage> TrImages { get; set; }

    public virtual DbSet<TrOrder> TrOrders { get; set; }

    public virtual DbSet<TrOrderBatch> TrOrderBatches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnumLookUp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EnumLookUp");

            entity.HasIndex(e => new { e.EnumName, e.EnumId }, "UQ__EnumLook__AB52D1D259B5A9DE").IsUnique();

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

        modelBuilder.Entity<SalesBand>(entity =>
        {
            entity.HasKey(e => e.SalesBandId).HasName("PK__SalesBan__D823A91596BA2564");

            entity.ToTable("SalesBand");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SalesBand1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("SalesBand");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StoreSalesBand>(entity =>
        {
            entity.HasKey(e => e.StoreSalesBandId).HasName("PK__StoreSal__19B5EBDFB48FC744");

            entity.ToTable("StoreSalesBand");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.SalesBand).WithMany(p => p.StoreSalesBands)
                .HasForeignKey(d => d.SalesBandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreSalesBand_SalesBandId");
        });

        modelBuilder.Entity<TrCart>(entity =>
        {
            entity.HasKey(e => e.TrCartId).HasName("PK__TrCart__6B0492B26A4CD8B9");

            entity.ToTable("TrCart");

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
            entity.HasKey(e => e.TrImageId).HasName("PK__TrImage__D6BA17141620212E");

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
            entity.HasKey(e => e.TrOrderId).HasName("PK__TrOrder__11569D3ECA14E949");

            entity.ToTable("TrOrder");

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
            entity.Property(e => e.Plu)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SalesBandPluCappedSnapshot).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(200);
            entity.Property(e => e.WeightCost).HasColumnType("decimal(5, 2)");

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
            entity.HasKey(e => e.TrOrderBatchId).HasName("PK__TrOrderB__DC57238D976D9BC9");

            entity.ToTable("TrOrderBatch");

            entity.HasIndex(e => e.StoreId, "idx_store");

            entity.Property(e => e.CostThresholdSnapshot).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalCostUponApproval).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
