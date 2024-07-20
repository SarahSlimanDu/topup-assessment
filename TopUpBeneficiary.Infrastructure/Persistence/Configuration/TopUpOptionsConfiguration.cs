using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate.ValueObjects;

namespace TopUpBeneficiary.Infrastructure.Persistence.Configuration
{
    public class TopUpOptionsConfiguration : IEntityTypeConfiguration<TopUpOption>
    {
        public void Configure(EntityTypeBuilder<TopUpOption> builder)
        {
            builder.ToTable("TopUpOptions");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedNever()
                .HasConversion(
                  o => o.Value,
                  value => TopUpOptionId.Create(value));
        }
    }
}
