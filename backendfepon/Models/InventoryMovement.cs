namespace backendfepon.Models
{
    public class InventoryMovement
    {
        public int Movement_Id { get; set; }
        public int Product_Id { get; set; }
        public int State_Id { get; set; }

        //This belongs to the movement type ID
        public int Inventory_Movement_Id { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }

        public InventoryMovementType InventoryMovementType { get; set; }

       public  State State { get; set; }

        public Product Product { get; set; }
    }
}