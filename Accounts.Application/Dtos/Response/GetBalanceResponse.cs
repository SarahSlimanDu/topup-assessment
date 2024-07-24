namespace Accounts.Application.Dtos.Response;

public sealed record GetBalanceResponse
{
    public string AccountIban { get; set; }
    public decimal Balance { get; set; }   
}
