
using Commons.Errors;

namespace TopUpBeneficiary.Domain.Errors
{
    public static class BeneficiaryErrors
    {
        public static Error NotFoundById() => new(
           "Beneficiary.NotFound", "The beneficiary did not found", ErrorTypes.NotFound);
    }
}
