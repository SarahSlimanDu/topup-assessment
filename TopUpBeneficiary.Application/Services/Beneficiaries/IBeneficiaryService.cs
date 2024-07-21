using Commons.Errors;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Dtos.Response;

namespace TopUpBeneficiary.Application.Services.Beneficiaries
{
    public interface IBeneficiaryService
    {
        Task<Result> AddBeneficiary(AddBeneficiaryDto request);
        Task<Result<IList<BeneficiaryDto>?>> GetBeneficiaries(Guid userId);
    }
}
