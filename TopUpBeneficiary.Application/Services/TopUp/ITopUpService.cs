using Commons.Errors;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Dtos.Response;

namespace TopUpBeneficiary.Application.Services.TopUp
{
    public interface ITopUpService
    {
        Task<Result<IList<TopUpOptionsDto>>> GetTopUpOptions();
        Task<Result> TopUpBeneficiary(TopUpDto topUpRequest);
    }
}
