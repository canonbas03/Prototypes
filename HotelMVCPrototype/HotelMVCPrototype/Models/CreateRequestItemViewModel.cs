using System.ComponentModel.DataAnnotations;

namespace HotelMVCPrototype.Models
{
    public class CreateRequestItemViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        public IFormFile? Image { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
