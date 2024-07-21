using Commons.Errors;
using TopUpBeneficiary.Application.SyncDataService.WebService.Response;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Application.SyncDataService.WebService.Client
{
    public interface IAccountExternalService
    {
        Task<Result<GetBalanceResponse>> GetBalance(AccountId accountId);
        Task DebitBalance(AccountId accountId);
    }
}
