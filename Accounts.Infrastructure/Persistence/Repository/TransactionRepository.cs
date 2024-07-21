using Accounts.Domain.Interface;
using Accounts.Domain.TransactionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.Persistence.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private DbSet<Transaction> _dbSet;

        public TransactionRepository(ApplicationDbContext context)
        {
            _dbSet = context.Set<Transaction>();
        }
        public void Add(Transaction transaction)
        {
            _dbSet.Add(transaction);    
        }

    }
}
