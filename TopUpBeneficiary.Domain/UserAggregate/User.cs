using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Commons.Model;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Domain.UserAggregate
{
    public sealed class User : AggregateRoot<UserId>
    {
        public string Email { get; private set; }
        public bool IsVerified { get; private set; }
        public AccountId AccountId { get; private set; }    
        public DateTime CreatedDateTime { get; private set; }
        public DateTime UpdatedDateTime { get; private set; }

        private User(UserId id, string email, bool isVerified, AccountId accountId, DateTime createdDateTime, DateTime updatedDateTime) : base(id)
        {
            Email = email;
            IsVerified = isVerified;
            AccountId = accountId;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
        }

        public static User Create(string email, bool isVerified, AccountId accountId)
        {
            return new(UserId.CreateUnique(),
                       email,
                       isVerified,
                       accountId,
                       DateTime.UtcNow,
                       DateTime.UtcNow);
        }
    }
}
