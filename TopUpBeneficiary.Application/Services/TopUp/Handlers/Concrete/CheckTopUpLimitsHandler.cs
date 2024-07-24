using Commons.Errors;
using Microsoft.Extensions.Options;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Commons.Constants;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete
{
    public class CheckTopUpLimitsHandler : Handler
    {
        private readonly ITopUpTransactionRepository _topUpRepository;
        private readonly AppConstants _appConstants;
        public CheckTopUpLimitsHandler(ITopUpTransactionRepository topUpRepository, IOptions<AppConstants> constants)
        {
            _topUpRepository = topUpRepository;
            _appConstants = constants.Value;
        }

        public override async Task<Result> HandleAsync(User user, Beneficiary beneficiary, int topUpAmount, int charge)
        {
            var sumTopUpAmount = await _topUpRepository.SumTopUpsInCurrentMonthPerBeneficiary(user.Id, beneficiary.Id);
            var sumTopUpForUserBeneficiaries = await _topUpRepository.SumTopUpsInCurrentMonthForAllBeneficiaries(user.Id);

            if (user.IsVerified && sumTopUpAmount + topUpAmount > _appConstants.MonthlyLimitPerBeneficiary_VerifiedUser)
            {
                return Result.Failure(TopUpTransactionErrors.ExceedBeneficiaryLimit());
            }
            else if (!user.IsVerified && sumTopUpAmount + topUpAmount > _appConstants.MonthlyLimitPerBeneficiary_UnVerifiedUser)
            {

                return Result.Failure(TopUpTransactionErrors.ExceedBeneficiaryLimit());
            }

            if (sumTopUpForUserBeneficiaries + topUpAmount > _appConstants.MonthlyLimit)
            {
                return Result.Failure(TopUpTransactionErrors.ExceedMonthlyLimit());
            }

            return await base.HandleAsync(user, beneficiary, topUpAmount, charge);
        }
    }
}
