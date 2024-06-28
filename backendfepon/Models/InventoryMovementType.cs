﻿namespace backendfepon.Models
{
    public class InventoryMovementType
    {
        public int Movement_Type_Id { get; set; }
        public string Movement_Type_Name { get; set; }
        public bool Stock_Increase { get; set; }

        public ICollection<InventoryMovement> InventoryMovements { get; set; }
      
    }
}
