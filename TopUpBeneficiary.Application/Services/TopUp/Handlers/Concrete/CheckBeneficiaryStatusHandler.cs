using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete
{
    public class CheckBeneficiaryStatusHandler : Handler
    {
        public override async Task HandleAsync(User user, Beneficiary beneficiary, int topUpAmount)
        {
            //check beneficiary status.//return true if active

            if (!beneficiary.IsActive)
                throw new Exception();
            else
                await base.HandleAsync(user, beneficiary, topUpAmount);
        }
    }
}
