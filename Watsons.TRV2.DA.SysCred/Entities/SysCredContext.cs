using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Watsons.TRV2.DA.SysCred.Entities;

public partial class SysCredContext : DbContext
{
    public SysCredContext()
    {
    }

    public SysCredContext(DbContextOptions<SysCredContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MfaApplication> MfaApplications { get; set; }

    public virtual DbSet<MfaUser> MfaUsers { get; set; }

    public virtual DbSet<MfaUserLogin> MfaUserLogins { get; set; }

    public virtual DbSet<Otptracking> Otptrackings { get; set; }

    public virtual DbSet<UserApplicationMapping> UserApplicationMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MfaApplication>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MfaApplication");

            entity.Property(e => e.ApplicationName).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Url).HasMaxLength(250);
        });

        modelBuilder.Entity<MfaUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MfaUser");

            entity.HasIndex(e => e.Username, "IX_MfaUser").IsUnique();

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Username).HasMaxLength(250);
        });

        modelBuilder.Entity<MfaUserLogin>(entity =>
        {
            entity.HasKey(e => e.UserApplicationId).HasName("PK__MfaUserL__BB6B62CD1A264A4F");

            entity.ToTable("MfaUserLogin");

            entity.Property(e => e.UserApplicationId)
                .ValueGeneratedNever()
                .HasColumnName("UserApplicationID");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Otptracking>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OTPTracking");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Otp)
                .HasMaxLength(6)
                .HasColumnName("OTP");
            entity.Property(e => e.OtpTimeout)
                .HasColumnType("datetime")
                .HasColumnName("OTP_Timeout");
            entity.Property(e => e.SessionIdentifier).HasMaxLength(100);
        });

        modelBuilder.Entity<UserApplicationMapping>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserApplicationMapping");

            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
