using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete
{
    public class CheckUserBalanceHandler : Handler
    {
        public override async Task HandleAsync(User user, Beneficiary beneficiary, int topUpAmount)
        {
            //we should call external Api to get the balance
            var userAccountBalance = 90;
            if (userAccountBalance < topUpAmount)
            {
                throw new Exception();
            }
            else
            {
                await base.HandleAsync(user, beneficiary, topUpAmount);
            }
        }
    }
}
