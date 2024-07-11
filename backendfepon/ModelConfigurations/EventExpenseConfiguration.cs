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

            // Many-to-many relationship configuration
            modelBuilder.HasMany(e => e.Providers)
                .WithMany(p => p.EventExpenses)
                .UsingEntity<Dictionary<string, object>>(
                    "EventExpenseProvider",
                    j => j
                        .HasOne<Provider>()
                        .WithMany()
                        .HasForeignKey("ProvidersProvider_Id")
                        .HasConstraintName("FK_EventExpenseProvider_Provider"),
                    j => j
                        .HasOne<EventExpense>()
                        .WithMany()
                        .HasForeignKey("EventExpensesExpense_Id")
                        .HasConstraintName("FK_EventExpenseProvider_EventExpense")
                );



        }
    }
}
