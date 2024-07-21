using Accounts.Application.Dtos.Response;
using Accounts.Domain.AccountAggregate.ValueObjects;
using Accounts.Domain.Errors;
using Accounts.Domain.Interface;
using Commons.Errors;

namespace Accounts.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
                
            _accountRepository = accountRepository; 
        }
        public async Task DebitBalance(Guid accountId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<GetBalanceResponse>> GetBalance(Guid accountId)
        {
          
            var account = await _accountRepository.GetAccountById(AccountId.Create(accountId));

            if(account is null)
            {
                return Result.Failure<GetBalanceResponse>(AccountErrors.NotFoundById());
            }
            return Result.Success(new GetBalanceResponse { AccountId = account.Id.Value, Balance = account.Balance});

        }
    }
}
