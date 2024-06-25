using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.MyMaster.Models;

namespace Watsons.TRV2.DA.MyMaster.Entities;
public partial class MigrationContext : DbContext
{
    public MigrationContext()
    {
    }

    public MigrationContext(DbContextOptions<MigrationContext> options)
        : base(options)
    {
    }

    internal virtual DbSet<ShipmentItem> ShipmentItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<ShipmentItem>(entity =>
        {
            entity.HasKey(e => new { e.ShipmentNumber, e.ItemCode });

            entity.ToTable("ShipmentItem");
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
