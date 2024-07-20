namespace TopUpBeneficiary.Domain.Persistence.Interfaces.Commons
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById<TKey>(TKey id) where TKey : notnull;
        Task<IEnumerable<T>> GetAll();
        void Add(T entity);
        void Update(T entity);
    }
}
