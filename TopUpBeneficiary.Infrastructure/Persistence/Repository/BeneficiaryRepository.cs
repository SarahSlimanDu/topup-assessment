using Microsoft.EntityFrameworkCore;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;
using TopUpBeneficiary.Infrastructure.Persistence.Commons;

namespace TopUpBeneficiary.Infrastructure.Persistence.Repository
{
    public class BeneficiaryRepository : Repository<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Beneficiary>> GetByUserId(UserId userId)
        {
            return await _context.Set<Beneficiary>().Where(b => b.UserId == userId
                                                             && b.IsActive).ToListAsync();
        }
    }
}
