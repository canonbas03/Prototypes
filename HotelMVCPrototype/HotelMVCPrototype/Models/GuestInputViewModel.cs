using System.ComponentModel.DataAnnotations;

public class GuestInputViewModel
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "EGN is required")]
    public string EGN { get; set; }

    [Required(ErrorMessage = "Birthdate is required")]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; } = new DateTime(2000, 1, 1);

    [Required(ErrorMessage = "Please select a gender")]
    public string Sex { get; set; }

    [Required(ErrorMessage = "Please select a nationality")]
    public Nationality? Nationality { get; set; }

    public string? Phone { get; set; }
}
