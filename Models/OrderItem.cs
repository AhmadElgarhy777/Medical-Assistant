using Models;

public class OrderItem : ModelBase
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Foreign Keys
    public string OrderId { get; set; }
    public string InventoryId { get; set; }

    // Navigation Properties
    public Order Order { get; set; }
    public Inventory Inventory { get; set; }
}