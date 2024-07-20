using Accounts.Domain.AccountAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Commons.Model;

namespace Accounts.Domain.AccountAggregate
{
    public sealed class Account : Entity<AccountId>
    {
        public string Iban {  get; private set; }   
        public string Email { get; private set; }
        public string Password { get; private set; }  
        public decimal Balance { get; private set; }    
        public string Currency {  get; private set; }   
        public DateTime CreatedDateTime { get;private set; }
        public DateTime UpdatedDateTime { get; private set; }
        
        private Account(AccountId id,string iban, string email, string password, decimal balance, string currency, DateTime createdDateTime, DateTime updatedDateTime) : base(id) 
        {
            Iban = iban;
            Email = email;
            Password = password;
            Balance = balance;
            Currency = currency;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
        }

        public static Account Create(string iban, string email, string password, decimal balance, string currency)
        {
            return new(AccountId.CreateUnique(),
                       iban,
                       email,
                       password,
                       balance,
                       currency,
                       DateTime.UtcNow,
                       DateTime.UtcNow);
        }
    }
}
