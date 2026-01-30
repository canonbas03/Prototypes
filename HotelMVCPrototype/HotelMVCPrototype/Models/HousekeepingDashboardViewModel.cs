namespace HotelMVCPrototype.Models
{
    public class HousekeepingDashboardViewModel
    {
        // Module 1: room list
        public List<Room> Rooms { get; set; }

        // Module 2: room statistics
        public RoomStatisticsViewModel RoomStatistics { get; set; }

        //public List<RoomMapViewModel> RoomMap { get; set; } = [];

        public RoomMapPageViewModel RoomMapPage { get; set; }

        public List<CleaningLog> TodaysCleanings { get; set; } = new();

        public List<RoomIssue> OpenHousekeepingIssues { get; set; } = new();

    }
}
