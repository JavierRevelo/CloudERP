using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class EventStateConfiguration : IEntityTypeConfiguration<EventState>
    {
        public void Configure(EntityTypeBuilder<EventState> modelBuilder)
        {
            modelBuilder.ToTable("EVENT_STATE");
            modelBuilder.HasKey(p => p.Event_State_Id);
        }
    }
}
