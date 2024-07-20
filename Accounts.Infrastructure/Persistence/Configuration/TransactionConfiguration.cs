using Accounts.Domain.AccountAggregate.ValueObjects;
using Accounts.Domain.TransactionAggregate;
using Accounts.Domain.TransactionAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Persistence.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");
            builder.HasKey(t => t.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedNever()
                .HasConversion(
                  u => u.Value,
                  value => TransactionId.Create(value));

            builder.Property(u => u.AccountId)
                .ValueGeneratedNever()
                .HasConversion(
                  u => u.Value,
                  value => AccountId.Create(value));

            builder.HasOne(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountId);


        }
    }
}
