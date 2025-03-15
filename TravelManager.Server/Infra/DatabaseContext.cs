using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class DatabaseContext : DbContext
{
    public DbSet<NodeModel> Nodes { get; set; }
    public DbSet<RouteModel> Routes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RouteModel>()
            .HasOne(r => r.Origin)
            .WithMany(p => p.OriginRoutes)
            .HasForeignKey(r => r.OriginId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<RouteModel>()
            .HasOne(r => r.Destination)
            .WithMany(p => p.DestinationRoutes)
            .HasForeignKey(r => r.DestinationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
