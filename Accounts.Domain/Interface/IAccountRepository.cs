using Accounts.Domain.AccountAggregate.ValueObjects;

namespace Accounts.Domain.Interface
{
    public interface IAccountRepository
    {
        Task GetBalance(AccountId accountId);
    }
}
