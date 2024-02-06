using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Watsons.TRV2.DA.MyMaster.Entities;

public partial class MyMasterContext : DbContext
{
    public MyMasterContext()
    {
    }

    public MyMasterContext(DbContextOptions<MyMasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ItemMaster> ItemMasters { get; set; }

    public virtual DbSet<StoreMaster> StoreMasters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemMaster>(entity =>
        {
            entity.HasKey(e => e.Item);

            entity.ToTable("ItemMaster");

            entity.Property(e => e.Item).HasMaxLength(10);
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.ChildPlu)
                .HasMaxLength(10)
                .HasColumnName("Child_PLU");
            entity.Property(e => e.ClassName).HasMaxLength(100);
            entity.Property(e => e.Dept).HasMaxLength(10);
            entity.Property(e => e.DeptName).HasMaxLength(200);
            entity.Property(e => e.EcomItemDesc).HasMaxLength(600);
            entity.Property(e => e.GroupName).HasMaxLength(200);
            entity.Property(e => e.GroupNo).HasMaxLength(50);
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.Indent).HasMaxLength(10);
            entity.Property(e => e.ItemStatus).HasMaxLength(50);
            entity.Property(e => e.RetekItemDesc).HasMaxLength(600);
            entity.Property(e => e.ReturnAllowInd)
                .HasMaxLength(10)
                .HasColumnName("return_allow_ind");
            entity.Property(e => e.ReturnPolicy)
                .HasMaxLength(10)
                .HasColumnName("return_policy");
            entity.Property(e => e.Rtdflag)
                .HasMaxLength(50)
                .HasColumnName("RTDFlag");
            entity.Property(e => e.RtdlatestDate).HasColumnName("RTDLatestDate");
            entity.Property(e => e.Rtnstatus)
                .HasMaxLength(50)
                .HasColumnName("RTNStatus");
            entity.Property(e => e.SupInactiveInd)
                .HasMaxLength(10)
                .HasColumnName("sup_inactive_ind");
            entity.Property(e => e.SupplierCode).HasMaxLength(50);
            entity.Property(e => e.SupplierName).HasMaxLength(200);
            entity.Property(e => e.SupplierProductCode).HasMaxLength(50);
            entity.Property(e => e.TesterBarcode).HasMaxLength(100);
            entity.Property(e => e.Trid).HasColumnName("TRID");
            entity.Property(e => e.Wastage).HasMaxLength(200);
        });

        modelBuilder.Entity<StoreMaster>(entity =>
        {
            entity.HasKey(e => e.StoreId);

            entity.ToTable("StoreMaster");

            entity.Property(e => e.StoreId).ValueGeneratedNever();
            entity.Property(e => e.AreaName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.Pharmacy)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasMaxLength(150);
            entity.Property(e => e.StoreAbbreviate).HasMaxLength(50);
            entity.Property(e => e.StoreAddress1).HasMaxLength(500);
            entity.Property(e => e.StoreAddress2).HasMaxLength(500);
            entity.Property(e => e.StoreCity).HasMaxLength(50);
            entity.Property(e => e.StoreCloseDate).HasColumnType("datetime");
            entity.Property(e => e.StoreFormat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StoreName).HasMaxLength(200);
            entity.Property(e => e.StoreOpenDate).HasColumnType("datetime");
            entity.Property(e => e.StorePostCode).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
