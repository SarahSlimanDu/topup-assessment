using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiaryService.UnitTests.Fixtures
{
    public static class UserFixtures
    {
        public static User User = User.Create("test@test.com", true, "DE89370400440532013000");
        public static User NotVerifiedUser  = User.Create("test@test.com", false, "DE89370400440532013000");    
    }
}
