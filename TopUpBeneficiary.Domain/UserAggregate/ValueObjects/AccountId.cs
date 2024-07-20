using TopUpBeneficiary.Domain.Commons.Model;

namespace TopUpBeneficiary.Domain.UserAggregate.ValueObjects
{
    public sealed class AccountId : ValueObject
    {
        public Guid Value { get; }

        private AccountId(Guid value)
        {
            Value = value;
        }
        public static AccountId Create(Guid value)
        {
            return new(value);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
