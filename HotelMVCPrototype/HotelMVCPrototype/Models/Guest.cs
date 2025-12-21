using HotelMVCPrototype.Models;

public class Guest
{
    public int Id { get; set; }

    public int GuestAssignmentId { get; set; }
    public GuestAssignment GuestAssignment { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string EGN { get; set; }          // National ID
    public DateTime BirthDate { get; set; }

    public string Sex { get; set; }           // "Male", "Female", "Other"
    public string Nationality { get; set; }

    public string? Phone { get; set; }
}
