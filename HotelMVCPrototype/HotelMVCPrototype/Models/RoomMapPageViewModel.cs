namespace HotelMVCPrototype.Models
{
    public class RoomMapPageViewModel
    {
        public int CurrentFloor { get; set; }
        public RoomMapMode Mode { get; set; }

        public List<RoomMapViewModel> Rooms { get; set; } = [];
    }

    public enum RoomMapMode
    {
        Reception,
        Housekeeping
    }




}
