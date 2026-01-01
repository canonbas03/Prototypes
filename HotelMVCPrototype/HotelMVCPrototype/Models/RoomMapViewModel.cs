namespace HotelMVCPrototype.Models
{
    public class RoomMapViewModel
    {
        public int RoomId { get; set; }
        public int Number { get; set; }

        public double TopPercent { get; set; }
        public double LeftPercent { get; set; }
        public double WidthPercent { get; set; }
        public double HeightPercent { get; set; }

        public string StatusColor { get; set; }

        public bool IsDND { get; set; }
        public bool HasOpenRequest { get; set; }
    }

}
