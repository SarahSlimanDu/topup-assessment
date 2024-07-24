using Accounts.Application.Dtos.Request;
using Accounts.Application.Dtos.Response;
using Commons.Errors;

namespace Accounts.Application.Services
{
    public interface IAccountService
    {
        Task<Result<GetBalanceResponse>> GetBalance(string accountId);
        Task<Result> DebitBalance(DebitBalanceRequest request);
    }
}
