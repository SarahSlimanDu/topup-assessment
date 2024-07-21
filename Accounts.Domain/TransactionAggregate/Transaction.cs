using Accounts.Domain.AccountAggregate;
using Accounts.Domain.AccountAggregate.ValueObjects;
using Accounts.Domain.TransactionAggregate.ValueObjects;
using System.Security.Cryptography.X509Certificates;
using TopUpBeneficiary.Domain.Commons.Model;

namespace Accounts.Domain.TransactionAggregate
{
    public sealed class Transaction : Entity<TransactionId>
    {
        public AccountId AccountId { get; private set; }
        public string Type { get; private set; }
        public decimal Amount { get; private set; } 
        public DateTime CreatedDateTime { get; private set; }  
        public Account Account { get; private set; }    
        private Transaction(TransactionId id, AccountId accountId, string type, decimal amount,DateTime createdDateTime) : base(id)
        { 
            AccountId = accountId;
            Type = type;
            CreatedDateTime = createdDateTime;
            Amount = amount;    
        }

        public static Transaction Create(AccountId accountId, string type, decimal amount)
        {
            return new(TransactionId.CreateUnique(), accountId, type, amount,DateTime.UtcNow);
        }


        
    }
}
