using Microsoft.EntityFrameworkCore;

namespace HotelMVC.Models
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
    }
}
