
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate;

namespace TopUpBeneficiary.Application.Services.TopUp.ChainOfResponsibilities
{
    public class CheckTopUpLimits : ApplyTopUp
    {
        private readonly ITopUpTransactionRepository _topUpRepository;
        public CheckTopUpLimits(ITopUpTransactionRepository topUpRepository)
        {
                _topUpRepository = topUpRepository;
        }

        public override async Task HandleAsync(User user, Beneficiary beneficiary, int topUpAmount)
        {
            ///get sum of all top up actions during this month for this beneficiary
            
            var sumTopUpAmount = await _topUpRepository.SumTopUpsInCurrentMonthForUserPerBeneficiary(user.Id, beneficiary.Id);
            var sumTopUpForUserBeneficiaries = await _topUpRepository.SumTopUpsTnCurrentMonthForUserBeneficiaries(user.Id);

            if ((user.IsVerified && sumTopUpAmount >= 1000) || (!user.IsVerified && sumTopUpAmount >= 500)) //TODO: this shouldn't be hard coded
            {

                throw new Exception();
            }
            else if(sumTopUpForUserBeneficiaries >= 3000)
            {
                throw new Exception();
            }
            else
            {
                await base.HandleAsync(user, beneficiary, topUpAmount);
            }
        }
    }
}
