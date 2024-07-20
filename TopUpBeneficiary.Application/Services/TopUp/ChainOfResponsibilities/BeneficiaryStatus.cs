
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.ChainOfResponsibilities
{
    public class BeneficiaryStatus : ApplyTopUp
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
