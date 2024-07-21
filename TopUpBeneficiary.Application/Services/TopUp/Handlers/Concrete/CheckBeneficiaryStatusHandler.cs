using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete
{
    public class CheckBeneficiaryStatusHandler : Handler
    {
        public override async Task<Result> HandleAsync(User user, Beneficiary beneficiary, int topUpAmount)
        {
            if (!beneficiary.IsActive)
                return Result.Failure(UserErrors.NotFoundById());
            else
                return await base.HandleAsync(user, beneficiary, topUpAmount);
        }
    }
}
