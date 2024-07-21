
namespace TopUpBeneficiary.Application.SyncDataService.WebService.Response
{
    public sealed record ErrorResponse
    {
        public string StatusCode {  get; set; }
        public string Type { get; set; }    
    }
}
