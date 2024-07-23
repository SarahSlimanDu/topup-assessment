using Accounts.Application.Dtos.Request;
using Accounts.Application.Dtos.Response;
using Accounts.Domain.AccountAggregate.ValueObjects;
using Accounts.Domain.Enums;
using Accounts.Domain.Errors;
using Accounts.Domain.Interface;
using Accounts.Domain.TransactionAggregate;
using Commons.Errors;

namespace Accounts.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> DebitBalance(DebitBalanceRequest request)
        {
            var account = await _accountRepository.GetAccountById(AccountId.Create(request.AccountId));
            if (account is null)
            {
                return Result.Failure<GetBalanceResponse>(AccountErrors.NotFoundById());
            }

            if(account.Balance < request.DebitAmount)
            {
                return Result.Failure<GetBalanceResponse>(AccountErrors.NoEnoughBalance());
            }
            _transactionRepository.Add(Transaction.Create(AccountId.Create(request.AccountId),TransactionType.Debit.ToString(), request.DebitAmount));
         
            account.DebitBalance(request.DebitAmount);

            _accountRepository.UpdateAccount(account);

            await _unitOfWork.Save();

            return Result.Success();    
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
