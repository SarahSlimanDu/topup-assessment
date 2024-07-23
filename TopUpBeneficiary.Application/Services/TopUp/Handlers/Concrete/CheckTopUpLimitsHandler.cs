using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete
{
    public class CheckTopUpLimitsHandler : Handler
    {
        private readonly ITopUpTransactionRepository _topUpRepository;
        public CheckTopUpLimitsHandler(ITopUpTransactionRepository topUpRepository)
        {
            _topUpRepository = topUpRepository;
        }

        public override async Task<Result> HandleAsync(User user, Beneficiary beneficiary, int topUpAmount, int charge)
        {
            var sumTopUpAmount = await _topUpRepository.SumTopUpsInCurrentMonthPerBeneficiary(user.Id, beneficiary.Id);
            var sumTopUpForUserBeneficiaries = await _topUpRepository.SumTopUpsInCurrentMonthForAllBeneficiaries(user.Id);

            if (user.IsVerified && sumTopUpAmount >= 500 || !user.IsVerified && sumTopUpAmount >= 1000)
            {

                return Result.Failure(TopUpTransactionErrors.ExceedBeneficiaryLimit());
            }
            else if (sumTopUpForUserBeneficiaries >= 3000)
            {
                return Result.Failure(TopUpTransactionErrors.ExceedMonthlyLimit());
            }
            else
            {
                return await base.HandleAsync(user, beneficiary, topUpAmount, charge);
            }
        }
    }
}
