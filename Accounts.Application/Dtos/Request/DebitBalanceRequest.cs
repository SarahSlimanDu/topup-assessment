namespace Accounts.Application.Dtos.Request
{
    public sealed record DebitBalanceRequest
    {
        public string AccountIban {  get; set; }    
        public decimal DebitAmount {  get; set; }    
    }
}
