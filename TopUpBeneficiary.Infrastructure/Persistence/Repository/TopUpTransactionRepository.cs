using Microsoft.EntityFrameworkCore;
using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Commons.Enums;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.TopUpTransactionAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;
using TopUpBeneficiary.Infrastructure.Persistence.Commons;

namespace TopUpBeneficiary.Infrastructure.Persistence.Repository
{
    public class TopUpTransactionRepository : Repository<TopUpTransaction>, ITopUpTransactionRepository
    {
        public TopUpTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> SumTopUpsInCurrentMonthForUserPerBeneficiary(UserId userId, BeneficiaryId beneficiaryId)
        {
            return await _context.Set<TopUpTransaction>().Where(t => t.UserId == userId
                                                                  && t.BeneficiaryId == beneficiaryId
                                                                  && t.Status == TopUpTransactionStatus.Success.ToString()
                                                                  && t.CreatedDateTime.Date.Month == DateTime.UtcNow.Month)
                                                         .Include(t => t.TopUpOption)
                                                         .SumAsync(t => t.TopUpOption.Amount);
        }

        public async Task<int> SumTopUpsTnCurrentMonthForUserBeneficiaries(UserId userId)
        {
           return await _context.Set<TopUpTransaction>().Where(t => t.UserId == userId
                                                                 && t.Status == TopUpTransactionStatus.Success.ToString()
                                                                 && t.CreatedDateTime.Date.Month == DateTime.UtcNow.Month)
                                                        .Include(t => t.TopUpOption)
                                                        .SumAsync(t => t.TopUpOption.Amount);   
        }
    }
}
