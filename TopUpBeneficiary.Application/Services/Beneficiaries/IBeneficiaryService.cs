
using Commons.Errors;
using TopUpBeneficiary.Application.Dtos.Response;

namespace TopUpBeneficiary.Application.Services.Beneficiaries
{
    public interface IBeneficiaryService
    {
        Task AddBeneficiary();
        Task<Result<IList<BeneficiaryDto>?>> GetBeneficiaries(Guid userId);
    }
}
