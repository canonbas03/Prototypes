using System;
using System.Collections.Generic;
using System.Linq;

public class RouteService
{
    private readonly List<RoutePoint> _points;

    public RouteService()
    {
        // seed some example points with offsets (hh:mm:ss)
        _points = new List<RoutePoint>
        {
            new RoutePoint { Id = 1, Name = "Razgrad", StartOffset = TimeSpan.FromSeconds(20), EndOffset = TimeSpan.FromSeconds(30), Side = "both", Description = "Main hub of the city." , AudioFile = "razgrad_sonia.mp3",  ScheduledTime = "16:34"},
            new RoutePoint { Id = 2, Name = "Kaspichan", StartOffset = TimeSpan.FromSeconds(30), EndOffset = TimeSpan.FromSeconds(90), Side = "right", Description = "A scenic river", AudioFile = "river_bridge.mp3",   ScheduledTime = "16:35" },
            new RoutePoint { Id = 3, Name = "Provadia", StartOffset = TimeSpan.FromSeconds(80), EndOffset = TimeSpan.FromSeconds(140), Side = "left", Description = "Ruins from 14th century", AudioFile = "trainAnnouncement.mp3",   ScheduledTime = "16:36"},
            new RoutePoint { Id = 4, Name = "Ezerovo", StartOffset = TimeSpan.FromSeconds(150), EndOffset = TimeSpan.FromSeconds(220), Side = "right", Description = "Small lake and park.", AudioFile = "trainAnnouncement.mp3",   ScheduledTime = "16:37"},
            new RoutePoint { Id = 5, Name = "Varna", StartOffset = TimeSpan.FromSeconds(210), EndOffset = TimeSpan.FromSeconds(270), Side = "both", Description = "Good panorama", AudioFile = "trainAnnouncement.mp3",   ScheduledTime = "16:38" }
        };
    }

    public IReadOnlyList<RoutePoint> AllPoints() => _points;

    // Get the single active point for the provided offset, side and direction
    // If none found, returns null. Simple strategy: first match in seed order.
    public RoutePoint? GetActivePoint(TimeSpan offset, string seatSide, string direction)
    {
        // seatSide: "left", "right", or "both"
        // direction currently unused but available for future logic (e.g. mirror sides when going backward)

        var normalizedSeat = (seatSide ?? "both").ToLowerInvariant();
        var candidates = _points.Where(p => offset >= p.StartOffset && offset <= p.EndOffset);

        // If direction is backward, we might want to flip left/right — here we keep simple.
        return candidates.FirstOrDefault(p => p.Side == "both" || p.Side == normalizedSeat);
    }
}