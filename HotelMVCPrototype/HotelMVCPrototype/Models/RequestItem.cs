public class RequestItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ImagePath { get; set; }

    public bool IsActive { get; set; } = true;

    public int? MaxQuantity { get; set; }
}
