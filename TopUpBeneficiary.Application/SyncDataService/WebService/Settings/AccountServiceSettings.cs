namespace TopUpBeneficiary.Application.SyncDataService.WebService.Settings
{
    public class AccountServiceSettings
    {
        public string BaseUrl { get; set; }
        public Endpoints Endpoints { get; set; }
    }
    public class Endpoints
    {
        public string GetBalance { get; set; }
        public string DebitBalance { get; set; }
    }
}
