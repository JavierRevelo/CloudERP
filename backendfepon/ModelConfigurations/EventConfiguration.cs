using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace backendfepon.ModelConfigurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> modelBuilder)
        {
            modelBuilder.ToTable("EVENT");
            modelBuilder.HasKey(p => p.Event_Id);

            // Relationships
            modelBuilder.HasOne(e => e.State)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.State_Id);
            // Relationships
            modelBuilder.HasOne(e => e.Financial_Request)
                .WithOne(c => c.Events)
                .HasForeignKey<Event>(e => e.Financial_Request_Id);
            modelBuilder.HasOne(e => e.Permission)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.Permission_Id);

        }
    }
}
