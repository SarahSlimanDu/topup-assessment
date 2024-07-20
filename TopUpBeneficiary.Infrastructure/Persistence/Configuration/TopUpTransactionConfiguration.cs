using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.TopUpTransactionAggregate;
using TopUpBeneficiary.Domain.TopUpTransactionAggregate.ValueObjects;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Infrastructure.Persistence.Configuration
{
    public class TopUpTransactionConfiguration : IEntityTypeConfiguration<TopUpTransaction>
    {
        public void Configure(EntityTypeBuilder<TopUpTransaction> builder)
        {
            builder.ToTable("TopUpTransactions");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
               .ValueGeneratedNever()
               .HasConversion(
                 t => t.Value,
                 value => TopUpTransactionId.Create(value));

            builder.Property(t => t.UserId)
               .ValueGeneratedNever()
               .HasConversion(
                 t => t.Value,
                 value => UserId.Create(value));
            builder.Property(t => t.BeneficiaryId)
              .ValueGeneratedNever()
              .HasConversion(
                t => t.Value,
                value => BeneficiaryId.Create(value));

            builder.HasOne(t => t.User)
           .WithMany()  
           .HasForeignKey(t => t.UserId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Beneficiary)
                .WithMany() 
                .HasForeignKey(t => t.BeneficiaryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
