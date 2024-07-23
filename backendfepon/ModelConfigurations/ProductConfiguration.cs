using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


//Setup Product model configurations
namespace backendfepon.ModelConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            modelBuilder.ToTable("PRODUCT");
            modelBuilder.HasKey(p => p.Product_Id);
            modelBuilder
               .HasOne(p => p.State)
               .WithMany(s => s.Products)
               .HasForeignKey(p => p.State_Id);
            modelBuilder
               .HasOne(p => p.Category)
               .WithMany(s => s.Products)
               .HasForeignKey(p => p.Category_Id);

            // Many-to-many relationship configuration
            modelBuilder
               .HasOne(p => p.Provider)
               .WithMany(s => s.Products)
               .HasForeignKey(p => p.Provider_Id);
        }
    }
}