using TopUpBeneficiary.Domain.Commons.Model;

namespace Accounts.Domain.TransactionAggregate.ValueObjects
{
    public sealed class TransactionId : ValueObject
    {
        public Guid Value { get; }

        private TransactionId(Guid value)
        {
            Value = value;
        }
        public static TransactionId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static TransactionId Create(Guid value)
        {
            return new(value);
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

    }
}
