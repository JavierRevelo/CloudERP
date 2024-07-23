namespace backendfepon.Models;

public partial class Product
{
    public int Product_Id { get; set; }

    public int State_Id { get; set; }

    public int Category_Id { get; set; }

    public int Provider_Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? Economic_Value { get; set; }

    public int Quantity { get; set; }

    public string Label { get; set; } = null!;

    // Navigation property
    public State State { get; set; }

    public Category Category { get; set; }

    public Provider Provider { get; set; }

    public ICollection<InventoryMovement> InventoryMovements { get; set; }
}
