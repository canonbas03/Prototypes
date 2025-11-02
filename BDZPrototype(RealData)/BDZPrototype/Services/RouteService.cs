public class RouteService
{
    private readonly List<RoutePoint> _points;

    public RouteService()
    {
        _points = new List<RoutePoint>
        {
            new RoutePoint { Name = "Razgrad", Latitude=43.21697, Longitude=27.91625, RadiusMeters=30, AudioFile = "razgrad_sonia.mp3"},
            new RoutePoint { Name = "Kaspichan", Latitude=43.21585, Longitude=27.91609, RadiusMeters=30, AudioFile = "river_bridge.mp3"},
            new RoutePoint { Name = "Provadia", Latitude=43.21466, Longitude=27.91537, RadiusMeters=30, AudioFile = "trainAnnouncement.mp3"},
            new RoutePoint { Name = "Ezerovo", Latitude=43.21517, Longitude=27.91339, RadiusMeters=30, AudioFile = "trainAnnouncement.mp3"},
            new RoutePoint { Name = "Varna", Latitude=43.21604, Longitude=27.91544, RadiusMeters=30, AudioFile = "trainAnnouncement.mp3"}
        };
    }

    public IReadOnlyList<RoutePoint> AllPoints() => _points;
}
