using Commons.Errors;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete
{
    public class DebitUserBalanceHandler : Handler
    {
        private readonly IAccountClient _accountClient;
        public DebitUserBalanceHandler(IAccountClient accountClient)
        {
            _accountClient = accountClient;  
        }
        public override async Task<Result> HandleAsync(User user, Beneficiary beneficiary, int topUpAmount, int charge)
        {
            var debitBalance = new DebitBalanceDto(user.AccountId.Value, topUpAmount + charge);
            var result = await _accountClient.DebitBalance(debitBalance);

            return result;

        }
    }
}
