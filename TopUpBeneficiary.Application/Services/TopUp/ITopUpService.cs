

using Commons.Errors;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Application.Services.TopUp
{
    public interface ITopUpService
    {
        Task GetTopUpOptions();
        Task<Result> TopUpBeneficiary(TopUpRequest topUpRequest);
    }
}
