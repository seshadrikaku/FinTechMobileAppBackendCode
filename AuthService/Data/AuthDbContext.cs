using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<MobileUser> MobileUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MobileUser>(entity =>
            {
                entity.ToTable("MobileUsers");
                entity.HasKey(e => e.Id);

                // Unique filtered index — allows re-registration after soft delete
                entity.HasIndex(e => e.MobileNumber)
                    .IsUnique()
                    .HasFilter("[isDeleted] = 0");

                entity.HasIndex(e => e.MobileUserId).IsUnique();

                entity.Property(e => e.MobileNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.OtpAttempts).HasColumnName("OtpAttempts");
                entity.Property(e => e.RefreshTokenHash).HasColumnName("RefreshToken");
                entity.Property(e => e.LastKnownAppVersion).HasColumnName("Version");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.IsActive).HasColumnName("isActive");
                entity.Property(e => e.IsExistingUser).HasColumnName("isExistingUser");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Global soft-delete filter — deleted users are invisible to all queries
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
