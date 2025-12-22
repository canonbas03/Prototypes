using System.ComponentModel.DataAnnotations;

public class MenuItemCreateViewModel
{
    [Required]
    public string Name { get; set; }

    [Range(0, 1000)]
    public decimal Price { get; set; }

    public MenuCategory Category { get; set; }

    public bool IsVegan { get; set; }

    public IFormFile? Image { get; set; }
}
