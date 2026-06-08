using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniNest.API.Models.RoomateFinder;
using UniNest.API.Models.User;

namespace UniNest.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<RoommateProfile> RoommateProfiles { get; set; } = null!;
        public DbSet<RoomListing> RoomListings { get; set; } = null!;
        public DbSet<ListingLocation> ListingLocations { get; set; } = null!;
        public DbSet<ListingImage> ListingImages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // RoomListing configuration
            builder.Entity<RoomListing>(eb =>
            {
                eb.HasKey(r => r.Id);
                eb.Property(r => r.MonthlyRent).HasColumnType("numeric(10,2)");
                eb.Property(r => r.CreatedAt).HasDefaultValueSql("now()");
                eb.Property(r => r.IsActive).HasDefaultValue(true);
                eb.HasIndex(r => r.MonthlyRent);
                eb.HasIndex(r => r.PreferredGender);
                eb.HasIndex(r => r.CreatedAt);
                eb.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                eb.HasOne(r => r.Location)
                    .WithOne(l => l.RoomListing)
                    .HasForeignKey<ListingLocation>(l => l.RoomListingId)
                    .OnDelete(DeleteBehavior.Cascade);
                eb.HasMany(r => r.Images)
                    .WithOne(i => i.RoomListing)
                    .HasForeignKey(i => i.RoomListingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RoommateProfile configuration
            builder.Entity<RoommateProfile>(eb =>
            {
                eb.HasKey(p => p.Id);
                eb.HasOne(p => p.User)
                    .WithOne()
                    .HasForeignKey<RoommateProfile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ListingImage configuration
            builder.Entity<ListingImage>(eb =>
            {
                eb.HasKey(i => i.Id);
                eb.Property(i => i.Order).HasDefaultValue(0);
                // Ensure at most one primary image per listing (Postgres partial index)
                eb.HasIndex(i => new { i.RoomListingId, i.IsPrimary })
                    .HasFilter("\"IsPrimary\" = TRUE")
                    .IsUnique();
            });

            // Map enums to string for readability
            builder.Entity<RoomListing>().Property(r => r.PreferredGender).HasConversion<string>();
        }
    }
}
