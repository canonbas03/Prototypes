using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Identity;

public class SecurityIncident
{
    public int Id { get; set; }

    public SecurityIncidentType Type { get; set; }

    public string? Description { get; set; }

    public int? RoomId { get; set; }
    public Room? Room { get; set; }

    public string ReportedByUserId { get; set; }
    public IdentityUser ReportedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ResolvedAt { get; set; }

    public SecurityIncidentStatus Status { get; set; }
        = SecurityIncidentStatus.New;
}
