
namespace TopUpBeneficiary.Application.SyncDataService.WebService.Client
{
    public class CustomHttpClientHandler : HttpClientHandler
    {
        public CustomHttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        }
    }
}
