using HotelMVCPrototype.Models;

public class RoomHistoryViewModel
{
    public int RoomId { get; set; }
    public int RoomNumber { get; set; }

    public List<GuestAssignment> Assignments { get; set; } = new();
}
