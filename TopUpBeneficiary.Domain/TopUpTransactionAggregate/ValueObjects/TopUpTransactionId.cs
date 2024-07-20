using TopUpBeneficiary.Domain.Commons.Model;

namespace TopUpBeneficiary.Domain.TopUpTransactionAggregate.ValueObjects
{
    public sealed class TopUpTransactionId : ValueObject
    {
        public Guid Value { get; }

        private TopUpTransactionId(Guid value)
        {
            Value = value;
        }

        public static TopUpTransactionId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static TopUpTransactionId Create(Guid value)
        {
            return new(value);
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
