using System.ComponentModel.DataAnnotations;

public enum MenuCategory
{
    Food = 1,
    [Display(Name = "Alcoholic Beverages")]
    AlcoholicBeverage = 2,
    [Display(Name = "Non-Alcoholic Beverages")]
    NonAlcoholicBeverage = 3
}
