
using Commons.Errors;

namespace TopUpBeneficiary.Domain.Errors
{
    public static class UserErrors
    {
        public static Error NotFoundById() => new(
            "Users.NotFound", "The user did not found", ErrorTypes.NotFound);

        public static Error NoEnoughBalance() => new(
            "Users.NoEnoughBalance", "The user account doesn't contain enough balance", ErrorTypes.Conflict);

        public static Error AccountNotFound() => new(
            "Users.AccountNotFound", "The user account is not found", ErrorTypes.Conflict); 

    }
}
