namespace TopUpBeneficiary.Domain.Persistence.Interfaces.Commons
{

    public interface IUnitOfWork : IDisposable
    {
        Task Save();

    }
}
