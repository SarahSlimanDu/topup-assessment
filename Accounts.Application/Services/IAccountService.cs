using Accounts.Application.Dtos.Response;
using Commons.Errors;

namespace Accounts.Application.Services
{
    public interface IAccountService
    {
        Task<Result<GetBalanceResponse>> GetBalance(Guid accountId);
        Task DebitBalance(Guid accountId, decimal amount);
    }
}
