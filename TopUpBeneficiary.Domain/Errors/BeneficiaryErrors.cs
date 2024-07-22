
using Commons.Errors;

namespace TopUpBeneficiary.Domain.Errors
{
    public static class BeneficiaryErrors
    {
        public static Error NotFoundById() => new(
           "Beneficiary.NotFound", "The beneficiary did not found", ErrorTypes.NotFound);

        public static Error NotActive() => new(
            "Beneficiary.NotActive", "The Beneficiary is not active", ErrorTypes.Conflict);

        public static Error CountLimitReached() => new(
            "Beneficiary.CountLimitReached", "User have reached the limit of active beneficiaries", ErrorTypes.Conflict);

        public static Error DuplicateNickName() => new(
            "Beneficiary.DuplicateNickName", "User already has beneficiary with same nick name", ErrorTypes.Conflict);

        public static Error DuplicatePhoneNumber() => new(
            "Beneficiary.DuplicatePhoneNumber", "User already has beneficiary with same phone number", ErrorTypes.Conflict);
    }
}
