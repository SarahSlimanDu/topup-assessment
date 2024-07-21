using Commons.Errors;
using Commons.HttpRequestBuild;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete
{
    public class CheckUserBalanceHandler : Handler
    {
        private readonly IAccountExternalService _accountExternalService;
        public CheckUserBalanceHandler(IAccountExternalService accountExternalService)
        {
               _accountExternalService = accountExternalService;
        }

        public override async Task<Result> HandleAsync(User user, Beneficiary beneficiary, int topUpAmount)
        {
            var result = await _accountExternalService.GetBalance(user.AccountId);

            if (result.IsSuccess)
            {
                if (result.Value.Balance < topUpAmount)
                {
                    return Result.Failure(UserErrors.NoEnoughBalance());
                }
                else
                {
                    return await base.HandleAsync(user, beneficiary, topUpAmount);
                }
            }
            else
                return result.Error;
        }
    }
}
