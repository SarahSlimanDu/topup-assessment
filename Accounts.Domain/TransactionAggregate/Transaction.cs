using Accounts.Domain.AccountAggregate;
using Accounts.Domain.AccountAggregate.ValueObjects;
using Accounts.Domain.TransactionAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Commons.Model;

namespace Accounts.Domain.TransactionAggregate
{
    public sealed class Transaction : Entity<TransactionId>
    {
        public AccountId AccountId { get; private set; }
        public string Type { get; private set; }
        public DateTime CreatedDateTime { get; private set; }  
        public Account Account { get; private set; }    
        private Transaction(TransactionId id, AccountId accountId, string type, DateTime createdDateTime) : base(id)
        { 
            AccountId = accountId;
            Type = type;
            CreatedDateTime = createdDateTime;
        }

        public static Transaction Create(AccountId accountId, string type)
        {
            return new(TransactionId.CreateUnique(), accountId, type, DateTime.UtcNow);
        }


        
    }
}
