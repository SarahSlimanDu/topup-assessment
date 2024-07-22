namespace TopUpBeneficiary.Domain.Persistence.Interfaces.Commons
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById<TKey>(TKey id) where TKey : notnull;
        Task<IEnumerable<T>> GetAll();
        T Add(T entity);
        void Update(T entity);
    }
}
