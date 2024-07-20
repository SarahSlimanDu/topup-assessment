using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Infrastructure.Persistence.Configuration
{
    public class BeneficiaryConfiguration : IEntityTypeConfiguration<Beneficiary>
    {
        public void Configure(EntityTypeBuilder<Beneficiary> builder)
        {
            builder.ToTable("Beneficiaries");
            builder.Property(b => b.NickName).HasMaxLength(20);
            builder.Property(b => b.PhoneNumber).HasMaxLength(10);

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedNever()
                .HasConversion(
                  b => b.Value,
                  value => BeneficiaryId.Create(value));

            builder.Property(b => b.UserId)
               .ValueGeneratedNever()
               .HasConversion(
                 b => b.Value,
                 value => UserId.Create(value));

            builder.HasOne(b => b.User)
                  .WithMany()
                  .HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
