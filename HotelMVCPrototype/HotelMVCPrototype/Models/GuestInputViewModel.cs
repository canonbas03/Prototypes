using System.ComponentModel.DataAnnotations;

public class GuestInputViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EGN { get; set; }
    [Display(Name = "Birthdate")]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; } = new DateTime(2000, 1, 1);
    public string Sex { get; set; }

    [Display(Name = "Nationality")]
    public Nationality? Nationality { get; set; }
    public string? Phone { get; set; }
}
