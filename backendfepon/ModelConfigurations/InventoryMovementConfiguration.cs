using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class InventoryMovementConfiguration : IEntityTypeConfiguration<InventoryMovement>
    {
        public void Configure(EntityTypeBuilder<InventoryMovement> modelBuilder)
        {
            modelBuilder.ToTable("INVENTORY_MOVEMENT");
            modelBuilder.HasKey(p => p.Movement_Id);
            modelBuilder
               .HasOne(p => p.Product)
               .WithMany(s => s.InventoryMovements)
               .HasForeignKey(p => p.Product_Id);


            modelBuilder
               .HasOne(p => p.InventoryMovementType)
               .WithMany(s => s.InventoryMovements)
               .HasForeignKey(p => p.Inventory_Movement_Id);

            modelBuilder
              .HasOne(p => p.State)
              .WithMany(s => s.InventoryMovements)
              .HasForeignKey(p => p.State_Id);
        }
    }
}
