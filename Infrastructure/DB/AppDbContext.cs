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
            .ToTable("venue");

        modelBuilder.Entity<Floor>()
            .ToTable("floor");

        modelBuilder.Entity<Node>()
            .ToTable("node");

        modelBuilder.Entity<Line>()
            .ToTable("line");

        modelBuilder.Entity<Line>()
            .HasOne(l => l.FirstNode)
            .WithMany(n => n.LinesAsFirst)
            .HasForeignKey(l => l.FirstNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Line>()
            .HasOne(l => l.SecondNode)
            .WithMany(n => n.LinesAsSecond)
            .HasForeignKey(l => l.SecondNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }


}
