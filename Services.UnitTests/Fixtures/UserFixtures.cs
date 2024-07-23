using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiaryService.UnitTests.Fixtures
{
    public static class UserFixtures
    {
        public static User User = User.Create("test@test.com", true, AccountId.Create(Guid.NewGuid()));
        public static User NotVerifiedUser  = User.Create("test@test.com", false, AccountId.Create(Guid.NewGuid()));    
    }
}
