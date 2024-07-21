using Commons.Errors;

namespace Accounts.Domain.Errors
{
    public static class AccountErrors
    {
        public static Error NotFoundById() => new(
            "Account.NotFound", "The account did not found", ErrorTypes.NotFound);
    }
}
