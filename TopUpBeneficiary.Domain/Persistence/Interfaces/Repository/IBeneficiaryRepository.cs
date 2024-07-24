using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Domain.Persistence.Interfaces.Repository
{
    public interface IBeneficiaryRepository : IRepository<Beneficiary>
    {
        Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesByUserId(UserId userId);
        Task<Beneficiary?> GetBeneficiaryByIdAndUserId(BeneficiaryId beneficiaryId, UserId userId);
    }
}
