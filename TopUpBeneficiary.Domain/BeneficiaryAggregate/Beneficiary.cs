using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Commons.Model;
using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Domain.BeneficiaryAggregate
{
    public sealed class Beneficiary : AggregateRoot<BeneficiaryId>
    {
        public UserId UserId { get; private set; }
        public string PhoneNumber { get; private set; }
        public string NickName { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime UpdatedDateTime { get; private set; }
        public User User { get; private set; }
        private Beneficiary(BeneficiaryId id, UserId userId, string phoneNumber, string nickName, bool isActive, DateTime createdDateTime, DateTime updatedDateTime) : base(id)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
            NickName = nickName;
            IsActive = isActive;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
        }

        public static Beneficiary Create(UserId userId, string phoneNumber, string nickName)
        {
            return new(BeneficiaryId.CreateUnique(),
                        userId,
                        phoneNumber,
                        nickName,
                        true,
                        DateTime.UtcNow,
                        DateTime.UtcNow);
        }
    }
}
