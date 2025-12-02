using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Renting.Models;

namespace Renting.Data
{
    public class RentingContext : DbContext
    {
        public RentingContext(DbContextOptions<RentingContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Rental> Rentals { get; set; }

    }
}
