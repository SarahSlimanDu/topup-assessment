

using Commons.Errors;

namespace TopUpBeneficiary.Domain.Errors
{
    public static class TopUpTransactionErrors
    {
        public static Error ExceedBeneficiaryLimit() => new(
          "TopUpTransaction.ExceedBeneficiaryLimit", "User have reached the monthly top up limit for this beneficiary ", ErrorTypes.Conflict);

        public static Error ExceedMonthlyLimit() => new(
          "TopUpTransaction.ExceedMonthlyLimit", "User have reached the monthly top up limit", ErrorTypes.Conflict);
    }
}
