using System.ComponentModel.DataAnnotations;

namespace HotelMVCPrototype.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
