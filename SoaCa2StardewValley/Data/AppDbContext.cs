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
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<PlanEntry> PlanEntries => Set<PlanEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // required: configures the Identity tables

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

        // Crop <-> Season: many-to-many through CropSeason (composite key)
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

        // User 1 -> * Plans. Deleting a user removes their plans.
        modelBuilder.Entity<Plan>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(80).IsRequired();
            e.Property(p => p.Strategy).HasConversion<string>().HasMaxLength(30);

            e.HasOne(p => p.Owner)
                .WithMany(u => u.Plans)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Season is reference data: never cascade-delete plans if a season is removed.
            e.HasOne(p => p.StartSeason)
                .WithMany()
                .HasForeignKey(p => p.StartSeasonId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Plan 1 -> * PlanEntries. Deleting a plan removes its entries.
        modelBuilder.Entity<PlanEntry>(e =>
        {
            e.HasOne(pe => pe.Plan)
                .WithMany(p => p.Entries)
                .HasForeignKey(pe => pe.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Crop is reference data: block deleting a crop that saved plans still use.
            e.HasOne(pe => pe.Crop)
                .WithMany()
                .HasForeignKey(pe => pe.CropId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Seed(); // seasons, crops and crop-season links
    }
}