using Accounts.Domain.TransactionAggregate;

namespace Accounts.Domain.Interface
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);
    }
}
