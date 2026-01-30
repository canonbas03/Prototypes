public class CreateRoomIssueViewModel
{
    public int? RoomId { get; set; }
    public int? RoomNumber { get; set; }

    public IssueCategory Category { get; set; }

    // selected dropdown option
    public string TypeKey { get; set; } = "";

    public string? Description { get; set; }
}
