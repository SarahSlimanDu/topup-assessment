using TopUpBeneficiary.Domain.Commons.Model;

namespace TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects
{
    public sealed class BeneficiaryId : ValueObject
    {
        public Guid Value { get; }

        private BeneficiaryId(Guid value)
        {
            Value = value;
        }

        public static BeneficiaryId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static BeneficiaryId Create(Guid value)
        {
            return new(value);
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
