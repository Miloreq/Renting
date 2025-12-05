using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Renting.Models;

namespace Renting.Data
{
    public class RentingContext : IdentityDbContext<User>
    {
        public RentingContext(DbContextOptions<RentingContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
