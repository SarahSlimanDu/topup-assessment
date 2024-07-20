namespace Accounts.Application.Services
{
    public interface IAccountService
    {
        Task GetBalance(Guid accountId);
        Task DebitBalance(Guid accountId, decimal amount);
    }
}
