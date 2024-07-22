

using Commons.Errors;

namespace TopUpBeneficiary.Domain.Errors
{
    public static class TopUpOptionsErrors
    {
        public static Error NotFoundById() => new(
           "TopUpOption.NotFoundById", "The used topUp option did not found", ErrorTypes.NotFound);

        public static Error NoOptionsFound() => new(
            "TopUpOption.NoOptionsFound", "No Top Up Options had been found ", ErrorTypes.NotFound);
    }
}
