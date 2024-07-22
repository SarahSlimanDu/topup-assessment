using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Domain.Persistence.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
