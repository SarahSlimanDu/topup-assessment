using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Commons.Model;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate.ValueObjects;
using TopUpBeneficiary.Domain.TopUpTransactionAggregate.ValueObjects;
using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Domain.TopUpTransactionAggregate
{
    public sealed class TopUpTransaction : AggregateRoot<TopUpTransactionId>
    {
        public UserId UserId { get; private set; }
        public BeneficiaryId BeneficiaryId { get; private set; }
        public TopUpOptionId TopUpOptionId { get; private set; }    
        public int Charge {  get; private set; }    
        public string Status { get; private set; }  
        public DateTime CreatedDateTime { get; private set; }   
        public DateTime UpdatedDateTime { get; private set; }   
        public User User { get; private set; }  
        public Beneficiary Beneficiary { get; private set; } 
        public TopUpOption TopUpOption { get; private set; }    
        private TopUpTransaction(TopUpTransactionId id, UserId userId, BeneficiaryId beneficiaryId, TopUpOptionId topUpOptionId,int charge, string status, DateTime createdDateTime, DateTime updatedDateTime) : base(id)
        {
            UserId = userId;
            BeneficiaryId = beneficiaryId;
            TopUpOptionId = topUpOptionId;  
            Charge = charge;
            Status = status;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
        }

        public static TopUpTransaction Create(UserId userId, BeneficiaryId beneficiaryId, TopUpOptionId topUpOptionId, int charge, string status)
        {
            return new(TopUpTransactionId.CreateUnique(),
                       userId,
                       beneficiaryId,
                       topUpOptionId,
                       charge,
                       status,
                       DateTime.UtcNow,
                       DateTime.UtcNow);

        }
    }
}
