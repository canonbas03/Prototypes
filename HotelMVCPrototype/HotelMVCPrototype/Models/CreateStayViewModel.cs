public class CreateStayViewModel
{
    public int RoomId { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public List<GuestInputViewModel> Guests { get; set; } = new();
}
