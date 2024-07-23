using Commons.Errors;
using Commons.HttpRequestBuild;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.SyncDataService.WebService.Response;
using TopUpBeneficiary.Application.SyncDataService.WebService.Settings;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Application.SyncDataService.WebService.Client
{
    public class AccountClient : IAccountClient
    {
        private readonly HttpClient _httpClient;
        private readonly AccountServiceSettings _settings;
        public AccountClient(HttpClient httpClient, IOptions<AccountServiceSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }
        public async Task<Result<GetBalanceResponse>> GetBalance(AccountId accountId)
        {
            var request = HttpRequestBuilder.Create(_settings.BaseUrl,
                _settings.Endpoints.GetBalance.Replace("{accountId}", accountId.Value.ToString()), HttpMethod.Get).Build();

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
       
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException($"Received an empty response from the external service {nameof(GetBalance)}.");
            }
            else
            {
                var balanceResponse = JsonConvert.DeserializeObject<GetBalanceResponse>(content);
                return Result.Success(balanceResponse);
            }
        }

        public async Task<Result> DebitBalance(DebitBalanceDto debitBalance)
        {
            var body = JsonConvert.SerializeObject(debitBalance);
            var request = HttpRequestBuilder.Create(_settings.BaseUrl,
                _settings.Endpoints.DebitBalance, HttpMethod.Post).SetRequestBody(body).Build();

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return Result.Success();
        }
    }
}
