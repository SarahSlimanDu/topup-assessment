
using Commons.Errors;

namespace TopUpBeneficiary.Domain.Errors
{
    public static class UserErrors
    {
        public static Error NotFoundById() => new(
            "Users.NotFound", "The user did not found", ErrorTypes.NotFound);

    }
}
