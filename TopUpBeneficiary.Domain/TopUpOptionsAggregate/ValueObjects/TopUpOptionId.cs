using TopUpBeneficiary.Domain.Commons.Model;

namespace TopUpBeneficiary.Domain.TopUpOptionsAggregate.ValueObjects
{
    public sealed class TopUpOptionId : ValueObject
    {
        public Guid Value { get; }

        private TopUpOptionId(Guid value)
        {
            Value = value;
        }

        public static TopUpOptionId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static TopUpOptionId Create(Guid value)
        {
            return new(value);
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
