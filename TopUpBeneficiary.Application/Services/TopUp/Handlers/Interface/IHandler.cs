using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Interface
{
    public interface IHandler
    {
        Handler SetNext(Handler handler);
        Task<Result> HandleAsync(User user, Beneficiary beneficiary, int topUpAmount, int charge);
    }
}
