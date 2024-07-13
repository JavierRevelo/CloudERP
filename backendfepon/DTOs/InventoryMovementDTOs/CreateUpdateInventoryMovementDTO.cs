namespace backendfepon.DTOs.InventoryMovementDTOs
{
    public class CreateUpdateInventoryMovementDTO
    {
        public string inventory_Movement_Type_Name { get; set; }
        public string product_Name { get; set; }
        public int quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
