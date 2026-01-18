public class AuditLog
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Who
    public string? UserId { get; set; }          // Identity user id
    public string? UserName { get; set; }        // snapshot (User.Identity.Name)
    public string? Role { get; set; }            // optional snapshot

    // What
    public string Action { get; set; } = null!;  // "OrderCompleted", "GuestDeparted", etc.
    public string EntityType { get; set; } = null!; // "Order", "Room", "ServiceRequest"
    public int? EntityId { get; set; }

    // Extra info
    public string? Description { get; set; }     // human-friendly message
    public string? DataJson { get; set; }        // optional details (before/after)

    // Context (super useful)
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
