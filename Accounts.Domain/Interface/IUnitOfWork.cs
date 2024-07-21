
namespace Accounts.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save();
    }
}
