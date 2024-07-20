
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate;
using TopUpBeneficiary.Infrastructure.Persistence.Commons;

namespace TopUpBeneficiary.Infrastructure.Persistence.Repository
{
    public class TopUpOptionsRepository : Repository<TopUpOption>, ITopUpOptionsRepository
    {
        public TopUpOptionsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
