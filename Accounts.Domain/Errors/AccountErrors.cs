using Commons.Errors;

namespace Accounts.Domain.Errors
{
    public static class AccountErrors
    {
        public static Error NotFoundById() => new(
            "Account.NotFound", "The account did not found", ErrorTypes.NotFound);
        public static Error NoEnoughBalance() => new(
            "Account.NoEnoughBalance", "The account does not have enough balance", ErrorTypes.Conflict);
    }
}
