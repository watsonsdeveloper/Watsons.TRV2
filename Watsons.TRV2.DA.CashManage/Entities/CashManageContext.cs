using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Watsons.TRV2.DA.CashManage.Entities;

public partial class CashManageContext : DbContext
{
    public CashManageContext()
    {
    }

    public CashManageContext(DbContextOptions<CashManageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserStoreId> UserStoreIds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AI");

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Username);

            entity.ToTable("UserLogin");

            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(200);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
        });

        modelBuilder.Entity<UserStoreId>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserStoreId");

            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
