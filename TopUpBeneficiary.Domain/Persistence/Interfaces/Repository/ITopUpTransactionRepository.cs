using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.TopUpTransactionAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Domain.Persistence.Interfaces.Repository
{
    public interface ITopUpTransactionRepository : IRepository<TopUpTransaction>
    {
        Task<int> SumTopUpsInCurrentMonthForUserPerBeneficiary(UserId userId, BeneficiaryId beneficiaryId);
        Task<int> SumTopUpsTnCurrentMonthForUserBeneficiaries(UserId userId);
    }
}   
