using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoaCa2StardewValley.Models;

namespace SoaCa2StardewValley.Data;

// https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore?view=aspnetcore-10.0
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<Crop> Crops => Set<Crop>();
    public DbSet<CropSeason> CropSeasons => Set<CropSeason>();
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<PlanEntry> PlanEntries => Set<PlanEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Season>(e =>
        {
            e.Property(s => s.Name).HasMaxLength(20).IsRequired();
            e.HasIndex(s => s.Name).IsUnique();
        });

        modelBuilder.Entity<Crop>(e =>
        {
            e.Property(c => c.Name).HasMaxLength(60).IsRequired();
            e.HasIndex(c => c.Name).IsUnique();
        });

        // Crop <-> Season: many-to-many through CropSeason
        modelBuilder.Entity<CropSeason>(e =>
        {
            e.HasKey(cs => new { cs.CropId, cs.SeasonId });

            e.HasOne(cs => cs.Crop)
                .WithMany(c => c.CropSeasons)
                .HasForeignKey(cs => cs.CropId);

            e.HasOne(cs => cs.Season)
                .WithMany(s => s.CropSeasons)
                .HasForeignKey(cs => cs.SeasonId);
        });

        // User 1 -> * Farms
        modelBuilder.Entity<Farm>(e =>
        {
            e.Property(f => f.Name).HasMaxLength(80).IsRequired();

            e.HasOne(f => f.Owner)
                .WithMany(u => u.Farms)
                .HasForeignKey(f => f.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Farm 1 -> * Plans
        modelBuilder.Entity<Plan>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(80).IsRequired();
            e.Property(p => p.Strategy).HasConversion<string>().HasMaxLength(30);

            e.HasOne(p => p.Farm)
                .WithMany(f => f.Plans)
                .HasForeignKey(p => p.FarmId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(p => p.StartSeason)
                .WithMany()
                .HasForeignKey(p => p.StartSeasonId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Plan 1 -> * PlanEntries
        modelBuilder.Entity<PlanEntry>(e =>
        {
            e.HasOne(pe => pe.Plan)
                .WithMany(p => p.Entries)
                .HasForeignKey(pe => pe.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pe => pe.Crop)
                .WithMany()
                .HasForeignKey(pe => pe.CropId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Seed();
    }
}