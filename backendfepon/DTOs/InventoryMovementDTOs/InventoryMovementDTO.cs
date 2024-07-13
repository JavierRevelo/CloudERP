namespace backendfepon.DTOs.InventoryMovementDTOs
{
    public class InventoryMovementDTO
    {
        public int id { get; set; }

        public int stateid { get; set; }
        public string product { get; set; }
        public string movementType { get; set; }
        public int quantity { get; set; }
        public string date { get; set; }
    }
}
