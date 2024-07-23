using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete
{
    public class CheckUserBalanceHandler : Handler
    {
        private readonly IAccountClient _accountClient;
        public CheckUserBalanceHandler(IAccountClient accountClient)
        {
            _accountClient = accountClient;
        }

        public override async Task<Result> HandleAsync(User user, Beneficiary beneficiary, int topUpAmount, int charge)
        {
            var result = await _accountClient.GetBalance(user.AccountId);

            if (result.Value.Balance < topUpAmount + charge)
            {
                return Result.Failure(UserErrors.NoEnoughBalance());
            }
            else
            {
                return await base.HandleAsync(user, beneficiary, topUpAmount, charge);
            }

        }
    }
}
