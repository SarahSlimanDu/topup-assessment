using Microsoft.EntityFrameworkCore;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiary.Infrastructure.Persistence.Commons;

namespace TopUpBeneficiary.Infrastructure.Persistence.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
