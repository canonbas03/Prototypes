namespace HotelMVCPrototype.Models
{
    public class HousekeepingDashboardViewModel
    {
        // Module 1: room list
        public List<Room> Rooms { get; set; }

        // Module 2: room statistics
        public RoomStatisticsViewModel RoomStatistics { get; set; }
    }
}
