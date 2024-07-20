
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.ChainOfResponsibilities
{
    public interface IApplyTopUp
    {
        ApplyTopUp SetNext(ApplyTopUp handler);
        Task HandleAsync(User user, Beneficiary beneficiary, int topUpAmount);
    }
}
