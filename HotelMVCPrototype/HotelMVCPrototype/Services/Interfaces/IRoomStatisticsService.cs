using HotelMVCPrototype.Models;

namespace HotelMVCPrototype.Services.Interfaces
{
    public interface IRoomStatisticsService
    {
        Task<RoomStatisticsViewModel> GetStatisticsAsync();
    }

}
