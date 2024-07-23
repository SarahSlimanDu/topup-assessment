using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;

namespace TopUpBeneficiaryService.UnitTests.Fixtures
{
    public static class BeneficiaryFixtures
    {
        public static Beneficiary ActiveBeneficiary = Beneficiary.Create(UserFixtures.User.Id, "123", "Nick1", true);
        public static Beneficiary UnActiveBeneficiary = Beneficiary.Create(UserFixtures.User.Id, "123", "Nick1", false);
        public static List<Beneficiary> GetBeneficiaries() => new()

            {
             Beneficiary.Create( UserFixtures.User.Id, "123", "Nick1", true ),
             Beneficiary.Create( UserFixtures.User.Id, "124", "Nick2", true ),
             Beneficiary.Create( UserFixtures.User.Id, "125", "Nick3", true ),
             Beneficiary.Create( UserFixtures.User.Id, "126", "Nick4", true ),
             Beneficiary.Create( UserFixtures.User.Id, "127", "Nick5", true )
            };

        public static List<Beneficiary> GetBeneficiaryWithSameNickName() => new()
        {
            Beneficiary.Create( UserFixtures.User.Id, "123", "DuplicateNick",true ),
        };

        public static List<Beneficiary> GetBeneficiaryWithSamePhoneNumber() => new()
        {
            Beneficiary.Create( UserFixtures.User.Id, "123456", "UniqueNick", true ),
        };

        public static List<Beneficiary> GetUserBeneficiaries() => new()
        {
            Beneficiary.Create( UserFixtures.User.Id, "123", "Nick1", true ),
             Beneficiary.Create( UserFixtures.User.Id, "124", "Nick2", true  )
        };

        public static List<BeneficiaryDto> GetBeneficiaryDtos(List<Beneficiary> beneficiaries) => new()
         {
            new BeneficiaryDto { BeneficiaryId = beneficiaries[0].Id.Value, PhoneNumber = beneficiaries[0].PhoneNumber, NickName = beneficiaries[0].NickName },
            new BeneficiaryDto { BeneficiaryId = beneficiaries[1].Id.Value, PhoneNumber = beneficiaries[1].PhoneNumber, NickName = beneficiaries[1].NickName }
        };
    }
}
