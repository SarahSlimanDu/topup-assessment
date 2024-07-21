namespace Accounts.Application.Dtos.Response;

public sealed record GetBalanceResponse
{
    public Guid AccountId { get; set; }
    public decimal Balance { get; set; } //TODO: check if sending string is best practice and if yes i have to create decimal parser.   
}
