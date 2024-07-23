using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class EventExpenseConfiguration : IEntityTypeConfiguration<EventExpense>
    {
        public void Configure(EntityTypeBuilder<EventExpense> modelBuilder)
        {
            modelBuilder.ToTable("EVENT_EXPENSE");
            modelBuilder.HasKey(p => p.Expense_Id);

            // Relationships
            modelBuilder.HasOne(e => e.Responsible)
                .WithOne(c => c.EventExpense)
                .HasForeignKey<EventExpense>(e => e.Responsible_Id);

            modelBuilder.HasOne(e => e.Transaction)
                .WithOne(c => c.EventExpense)
                .HasForeignKey<EventExpense>(e => e.Transaction_Id);

            /*modelBuilder.HasOne(e => e.Event)
                .WithMany(c => c.EventExpenses)
                .HasForeignKey(e => e.Event_Id);*/




        }
    }
}
