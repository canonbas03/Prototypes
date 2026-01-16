using HotelMVCPrototype.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelMVCPrototype.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }


        public OrderStatus Status { get; set; } = OrderStatus.New;

        public ICollection<OrderItem> Items { get; set; }
    }
}
