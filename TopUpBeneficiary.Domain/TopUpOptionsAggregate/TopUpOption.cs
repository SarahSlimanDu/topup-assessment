using TopUpBeneficiary.Domain.Commons.Model;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate.ValueObjects;

namespace TopUpBeneficiary.Domain.TopUpOptionsAggregate
{
    public sealed class TopUpOption : AggregateRoot<TopUpOptionId>
    {
        public int Amount { get; private set; }
        public DateTime CreatedDateTime { get; private set; }   
        private TopUpOption(TopUpOptionId id, int amount, DateTime createdDateTime) : base(id)
        {
            Amount = amount;
            CreatedDateTime = createdDateTime;
        }

        public static TopUpOption Create(int amount)
        {
            return new(TopUpOptionId.CreateUnique(), amount, DateTime.UtcNow);
        }
    }
}
