using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class EventIncomeConfiguration : IEntityTypeConfiguration<EventIncome>
    {
        public void Configure(EntityTypeBuilder<EventIncome> modelBuilder)
        {
            modelBuilder.ToTable("EVENT_INCOME");
            modelBuilder.HasKey(p => p.Income_Id);
            /*
            // Relationships
            modelBuilder.HasOne(e => e.Event)
                .WithOne(c => c.EventIncome)
                .HasForeignKey<EventIncome>(e => e.Event_Id);*/

            modelBuilder.HasOne(e => e.Transaction)
               .WithOne(c => c.EventIncome)
               .HasForeignKey<EventIncome>(e => e.Transaction_Id);

        }
    }
}
