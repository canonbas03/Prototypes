using HotelMVCPrototype.Models;
using System.Collections.Generic;

namespace HotelMVCPrototype.Models
{
    public class ReceptionDashboardViewModel
    {
        // Module 1: room list
        public List<Room> Rooms { get; set; }

        // Module 2: room statistics
        public RoomStatisticsViewModel RoomStatistics { get; set; }

        // Side module (new)
        public List<ServiceRequest> NewRequests { get; set; }

        public List<RoomMapViewModel> RoomMap { get; set; }
    }
}
