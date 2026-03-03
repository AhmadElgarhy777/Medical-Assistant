using Models;

public class Order : ModelBase
{
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }

    // Foreign Keys
    public string PatientId { get; set; }
    public string PharmacyId { get; set; }

    // Navigation Properties
    public Patient Patient { get; set; }
    public Pharmacy Pharmacy { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public Invoice Invoice { get; set; }
}