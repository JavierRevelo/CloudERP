using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class FinancialRequestConfiguration : IEntityTypeConfiguration<FinancialRequest>
    {
        public void Configure(EntityTypeBuilder<FinancialRequest> modelBuilder)
        {
            modelBuilder.ToTable("FINANCIAL_REQUEST");
            modelBuilder.HasKey(p => p.Request_Id);

            // Relationships
            modelBuilder.HasOne(e => e.Financial_Request_State)
                .WithMany(c => c.FinancialRequests)
                .HasForeignKey(e => e.Request_Status_Id);
            modelBuilder.HasOne(e => e.State)
               .WithMany(c => c.FinancialRequests)
               .HasForeignKey(e => e.State_Id);



        }
    }
}
