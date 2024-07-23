using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Domain.Commons.Enums;
using TopUpBeneficiary.Domain.TopUpTransactionAggregate;

namespace TopUpBeneficiaryService.UnitTests.Fixtures
{
    public static class TopUpBeneficiaryFixtures
    {
        public static TopUpDto TopUpDto = new TopUpDto
        {
            UserId = Guid.NewGuid(),
            BeneficiaryId = Guid.NewGuid(),
            topUpOptionId = Guid.NewGuid(),
        };

        public static TopUpTransaction TopUpTransaction = TopUpTransaction.Create(UserFixtures.User.Id, 
            BeneficiaryFixtures.ActiveBeneficiary.Id, TopUpOptionsFixtures.TopUpOption.Id, 1, TopUpTransactionStatus.Pending.ToString());
    }
}
