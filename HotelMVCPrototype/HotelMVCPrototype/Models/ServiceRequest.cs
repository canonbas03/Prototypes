using HotelMVCPrototype.Models.Enums;

namespace HotelMVCPrototype.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.New;

        public ICollection<ServiceRequestItem> Items { get; set; } = new List<ServiceRequestItem>();

        public DateTime? CompletedAt { get; set; }

    }
}
