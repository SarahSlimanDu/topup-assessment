using Commons.Errors;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.SyncDataService.WebService.Response;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Application.SyncDataService.WebService.Client
{
    public interface IAccountClient
    {
        Task<Result<GetBalanceResponse>> GetBalance(AccountId accountId);
        Task<Result> DebitBalance(DebitBalanceDto debitBalance);
    }
}
