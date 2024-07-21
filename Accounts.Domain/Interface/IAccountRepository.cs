using Accounts.Domain.AccountAggregate;
using Accounts.Domain.AccountAggregate.ValueObjects;

namespace Accounts.Domain.Interface
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountById(AccountId accountId);
        void UpdateAccount(Account account);
    }
}
