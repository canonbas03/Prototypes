using System.ComponentModel.DataAnnotations;

public class CreateStayViewModel
{
    public int RoomId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Arrival Date")]
    public DateTime CheckInDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Departure Date")]
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

    [MinLength(1, ErrorMessage = "At least one guest is required.")]
    public List<GuestInputViewModel> Guests { get; set; } = new();
}
