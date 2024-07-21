using Accounts.Domain.AccountAggregate;
using Accounts.Domain.AccountAggregate.ValueObjects;
using Accounts.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.Persistence.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private DbSet<Account> _dbSet;

        public AccountRepository(ApplicationDbContext context)
        {
            _dbSet = context.Set<Account>();
        }
        public async Task<Account?> GetAccountById(AccountId accountId)
        {
            return await _dbSet.SingleOrDefaultAsync(a => a.Id == accountId);
        }

        public void UpdateAccount(Account account)
        {
            _dbSet.Update(account);
        }
    }
}
