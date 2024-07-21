using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Interface;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Base
{
    public class Handler : IHandler
    {
        private Handler? _nextHandler;
        public Handler SetNext(Handler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual async Task<Result> HandleAsync(User user, Beneficiary beneficiary, int topUpAmount)
        {
            if (_nextHandler != null)
            {
                return await _nextHandler.HandleAsync(user, beneficiary, topUpAmount);
            }

            else
                return Result.Failure(UserErrors.NotFoundById());
                
        }
    }
}
