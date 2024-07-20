

using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.ChainOfResponsibilities
{
    public class DebitUserAccount : ApplyTopUp
    {
        public override async Task HandleAsync(User user, Beneficiary beneficiary, int topUpAmount)
        {
            //we have to call external Api to debit the account/
            //if this call return 200 then every thing done successfully.

            var response = "200";

            if(response == "200")
            {
                //update the debit transaction status
               // await base.HandleAsync(user, beneficiary, topUpAmount);
            }

            else
            {
                throw new Exception();
            }

        }
    }
}
