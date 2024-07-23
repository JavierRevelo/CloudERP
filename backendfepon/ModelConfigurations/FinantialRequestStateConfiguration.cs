using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class FinantialRequestStateConfiguration : IEntityTypeConfiguration<FinancialRequestState>
    {
        public void Configure(EntityTypeBuilder<FinancialRequestState> modelBuilder)
        {
            modelBuilder.ToTable("FINANCIAL_REQUEST_STATE");
            modelBuilder.HasKey(p => p.Request_State_Id);

        }
    }
}
