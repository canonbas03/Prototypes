using HotelMVCPrototype.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelMVCPrototype.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public int Number { get; set; }

        [Required]
        [Display(Name = "Room Type")]
        public RoomType Type { get; set; }

        [Required]
        [Display(Name = "Status")]
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        [Display(Name = "Do Not Disturb")]
        public bool IsDND { get; set; } = false;

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        public ICollection<GuestAssignment> GuestAssignments { get; set; } = new List<GuestAssignment>();
    }
}
