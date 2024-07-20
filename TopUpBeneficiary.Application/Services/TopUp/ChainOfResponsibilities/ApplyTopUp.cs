

using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.ChainOfResponsibilities
{
    public class ApplyTopUp : IApplyTopUp
    {
        private ApplyTopUp? _nextHandler;
        public ApplyTopUp SetNext(ApplyTopUp handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual async Task HandleAsync(User user, Beneficiary beneficiary,int topUpAmount)
        {
            if (_nextHandler != null)
            {
                 await _nextHandler.HandleAsync(user, beneficiary, topUpAmount);
            }
        }
    }
}
