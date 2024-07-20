
using TopUpBeneficiary.Application.Dtos.Response;

namespace TopUpBeneficiary.Application.Services.Beneficiaries
{
    public interface IBeneficiaryService
    {
        Task AddBeneficiary();
        Task<IList<BeneficiaryDto>?> GetBeneficiaries(Guid userId);
    }
}
