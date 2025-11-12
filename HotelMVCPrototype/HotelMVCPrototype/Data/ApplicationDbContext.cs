using HotelMVCPrototype.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Unique room number
            builder.Entity<Room>()
                   .HasIndex(r => r.Number)
                   .IsUnique();
        }

        public DbSet<GuestAssignment> GuestAssignments { get; set; }

    }
}
