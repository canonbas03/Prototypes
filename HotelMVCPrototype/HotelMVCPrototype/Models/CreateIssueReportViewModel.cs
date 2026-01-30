using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class CreateIssueReportViewModel
{
    public int? RoomId { get; set; }
    public int? RoomNumber { get; set; }

    [Required]
    public IssueCategory Category { get; set; }

    [Required]
    public string TypeKey { get; set; } = ""; // selected option key (e.g. "WeirdNoise")

    public string? Description { get; set; }

    // for room dropdown when no roomId in URL
    public List<SelectListItem> Rooms { get; set; } = new();

    // for dynamic type dropdown (filled by JS, or you can pre-fill based on Category)
    public List<SelectListItem> Types { get; set; } = new();
}

public enum IssueCategory
{
    Housekeeping,
    Maintenance,
    Security
}
