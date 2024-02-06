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

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<TrCart> TrCarts { get; set; }

    public virtual DbSet<TrImage> TrImages { get; set; }

    public virtual DbSet<TrOrder> TrOrders { get; set; }

    public virtual DbSet<TrOrderBatch> TrOrderBatches { get; set; }

    public virtual DbSet<TrPlu> TrPlus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__DAD4F05E85AD559E");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandId).ValueGeneratedOnAdd();
            entity.Property(e => e.Brand1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Brand");
        });

        modelBuilder.Entity<TrCart>(entity =>
        {
            entity.HasKey(e => e.TrCartId).HasName("PK__TrCart__6B0492B242B4D8B6");

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
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Plu)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Reason)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Brand).WithMany(p => p.TrCarts)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrCart_BrandId");
        });

        modelBuilder.Entity<TrImage>(entity =>
        {
            entity.HasKey(e => e.TrImageId).HasName("PK__TrImage__D6BA17147A08762E");

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
            entity.HasKey(e => e.TrOrderId).HasName("PK__TrOrder__11569D3E1E27F247");

            entity.ToTable("TrOrder");

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
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Plu)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Reason)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(200);
            entity.Property(e => e.TrOrderBatchId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WeightCost).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.TrOrderBatch).WithMany(p => p.TrOrders)
                .HasForeignKey(d => d.TrOrderBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrOrder_TrOrderBatchId");
        });

        modelBuilder.Entity<TrOrderBatch>(entity =>
        {
            entity.HasKey(e => e.TrOrderBatchId).HasName("PK__TrOrderB__DC57238DE3643342");

            entity.ToTable("TrOrderBatch");

            entity.Property(e => e.TrOrderBatchId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Threshold).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.TotalCostUponApproval).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Brand).WithMany(p => p.TrOrderBatches)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrOrderBatch_BrandId");
        });

        modelBuilder.Entity<TrPlu>(entity =>
        {
            entity.HasKey(e => e.TrPluId).HasName("PK__TrPlu__36F4161CADDEF259");

            entity.ToTable("TrPlu");

            entity.HasIndex(e => e.StoreId, "idx_store");

            entity.Property(e => e.Barcode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Plu)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Reason)
                .HasMaxLength(30)
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
