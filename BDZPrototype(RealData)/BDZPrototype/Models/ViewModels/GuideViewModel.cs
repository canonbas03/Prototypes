using System.Collections.Generic;

public class GuideViewModel
{
    public List<RoutePoint> AllPoints { get; set; } = new List<RoutePoint>();
    public TimeSpan CurrentOffset { get; set; }
    public string SeatSide { get; set; } = "both";
    public string Direction { get; set; } = "forward";
    public RoutePoint? ActivePoint { get; set; }
    public bool UsingGps { get; set; }
}