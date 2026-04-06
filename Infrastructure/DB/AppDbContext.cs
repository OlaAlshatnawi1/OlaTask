using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure.DB;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Venue> Venues { get; set; }
    public DbSet<Floor> Floors { get; set; }
    public DbSet<Node> Nodes { get; set; }
    public DbSet<Line> Lines { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venue>()
            .ToTable("venue")
            .HasQueryFilter(v => !v.IsDeleted);

        modelBuilder.Entity<Floor>()
            .ToTable("floor")
            .HasQueryFilter(f => !f.IsDeleted);

        modelBuilder.Entity<Node>()
            .ToTable("node")
            .HasQueryFilter(n => !n.IsDeleted);

        modelBuilder.Entity<Line>()
            .ToTable("line")
            .HasQueryFilter(l => !l.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }


}
