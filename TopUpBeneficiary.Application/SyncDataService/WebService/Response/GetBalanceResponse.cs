namespace TopUpBeneficiary.Application.SyncDataService.WebService.Response
{
    public sealed record GetBalanceResponse
    {
        public Guid AccountId { get; set; }
        public decimal Balance { get; set; }
    }
}
