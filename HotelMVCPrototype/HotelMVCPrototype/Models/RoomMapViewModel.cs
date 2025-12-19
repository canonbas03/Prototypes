namespace HotelMVCPrototype.Models
{
    public class RoomMapViewModel
    {
        public int RoomId { get; set; }
        public int Number { get; set; }

        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string StatusColor { get; set; }
    }

}
