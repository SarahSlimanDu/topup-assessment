using Accounts.Domain.AccountAggregate;
using Accounts.Domain.AccountAggregate.ValueObjects;

namespace Accounts.Domain.Interface
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByIban(string accountIban);
        void UpdateAccount(Account account);
        Task<IEnumerable<Account>> GetAllAccounts();
        void Add(Account account);  
    }
}
