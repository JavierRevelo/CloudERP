using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class AssociationConfiguration : IEntityTypeConfiguration<Association>
    {
        public void Configure(EntityTypeBuilder<Association> modelBuilder)
        {
            modelBuilder.ToTable("ASSOCIATION");
            modelBuilder.HasKey(p => p.Association_Id);
            modelBuilder
               .HasOne(p => p.State)
               .WithMany(s => s.Associations)
               .HasForeignKey(p => p.State_Id);

        }
    }
}
