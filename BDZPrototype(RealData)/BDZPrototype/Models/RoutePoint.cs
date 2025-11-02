using System;

public class RoutePoint
{
    public int Id { get; set; }
    public string Name { get; set; } = ""; // e.g. "Old Fortress"

    // Offsets relative to journey start
    public TimeSpan StartOffset { get; set; }
    public TimeSpan EndOffset { get; set; }

    // Which side of the train the POI is on: "left", "right", or "both"
    public string Side { get; set; } = "both";

    // Optional short description
    public string Description { get; set; } = "";

    public string? AudioFile { get; set; }

    public string ScheduledTime { get; set; } = "";


}