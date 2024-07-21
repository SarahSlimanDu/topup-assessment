

using Commons.Errors;

namespace TopUpBeneficiary.Domain.Errors
{
    public static class TopUpOptionsErrors
    {
        public static Error NotFoundById() => new(
           "TopUpOption.NotFound", "The used topUp option did not found", ErrorTypes.NotFound);
    }
}
