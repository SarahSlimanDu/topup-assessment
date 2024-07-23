namespace Accounts.Application.Dtos.Request
{
    public sealed record DebitBalanceRequest
    {
        public Guid AccountId {  get; set; }    
        public decimal DebitAmount {  get; set; }    
    }
}
