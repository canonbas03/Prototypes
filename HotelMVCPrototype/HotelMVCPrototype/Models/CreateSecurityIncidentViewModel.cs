using HotelMVCPrototype.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelMVCPrototype.Models
{
    public class CreateSecurityIncidentViewModel
    {
        [Required]
        public SecurityIncidentType Type { get; set; }

        public int? RoomId { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }

}
