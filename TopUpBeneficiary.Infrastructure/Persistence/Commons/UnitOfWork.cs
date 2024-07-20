using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;

namespace TopUpBeneficiary.Infrastructure.Persistence.Commons
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
