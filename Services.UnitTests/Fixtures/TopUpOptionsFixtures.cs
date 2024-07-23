using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate;

namespace TopUpBeneficiaryService.UnitTests.Fixtures
{
    public static class TopUpOptionsFixtures
    {
        public static TopUpOption TopUpOption = TopUpOption.Create(10);
        public static List<TopUpOption> GetTopUpOptions() => new()
        {
            TopUpOption.Create(10),
            TopUpOption.Create(20)
        };

        public static List<TopUpOptionsDto> GetTopUpOptionsDtos(List<TopUpOption> topUpOptions) => new()
        {
            new TopUpOptionsDto { Id = topUpOptions[0].Id.Value, Amount = topUpOptions[0].Amount, CreatedDateTime = topUpOptions[0].CreatedDateTime },
            new TopUpOptionsDto { Id = topUpOptions[1].Id.Value, Amount = topUpOptions[1].Amount, CreatedDateTime = topUpOptions[1].CreatedDateTime}
        };

    }
}
