using Models;
using Models.Enums;

public class Booking : ModelBase
{
    public string NurseId { get; set; } = null!;
    public string PatientId { get; set; } = null!;
    public DateTime RequestDate { get; set; }
    public string? Status { get; set; }
    public string Address { get; set; }= null!;
    public string City { get; set; } = null!;
    public Governorate Governorate { get; set; }
}
