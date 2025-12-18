using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using HotelMVCPrototype.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Services
{

    public class RoomStatisticsService : IRoomStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public RoomStatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoomStatisticsViewModel> GetStatisticsAsync()
        {
            return new RoomStatisticsViewModel
            {
                Available = await _context.Rooms.CountAsync(r => r.Status == RoomStatus.Available),
                Occupied = await _context.Rooms.CountAsync(r => r.Status == RoomStatus.Occupied),
                Cleaning = await _context.Rooms.CountAsync(r => r.Status == RoomStatus.Cleaning),
                Maintenance = await _context.Rooms.CountAsync(r => r.Status == RoomStatus.Maintenance)
            };
        }
    }
}
