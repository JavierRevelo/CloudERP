using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class ResponsibleConfiguration : IEntityTypeConfiguration<Responsible>
    {
        public void Configure(EntityTypeBuilder<Responsible> modelBuilder)
        {
            modelBuilder.ToTable("EVENT_RESPONSIBLE");
            modelBuilder.HasKey(p => p.Responsible_Id);

            // Relationships
            modelBuilder.HasOne(e => e.AdministrativeMember)
                .WithOne(c => c.Responsible)
                .HasForeignKey<Responsible>(e => e.Administrative_Member_Id);
            /*
            modelBuilder.HasOne(e => e.Event)
                        .WithMany(c => c.Responsibles)
                        .HasForeignKey(e => e.Event_Id);*/
        }
    }
}
