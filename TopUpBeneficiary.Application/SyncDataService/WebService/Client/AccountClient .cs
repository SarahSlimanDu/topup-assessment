using Commons.Errors;
using Commons.HttpRequestBuild;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.SyncDataService.WebService.Response;
using TopUpBeneficiary.Application.SyncDataService.WebService.Settings;
using TopUpBeneficiary.Domain.Errors;
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
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                var balanceResponse = JsonConvert.DeserializeObject<GetBalanceResponse>(content);
                if (balanceResponse != null)
                    return Result.Success(balanceResponse);
                else
                    throw new Exception("Something went wrong");//TODO : re-check
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);

                if (errorResponse != null && errorResponse.StatusCode == ErrorTypes.NotFound.ToString())
                    return Result.Failure<GetBalanceResponse>(UserErrors.AccountNotFound());
                else
                    throw new Exception("Something went wrong");//TODO: check
            }
        }

        public async Task<Result> DebitBalance(DebitBalanceDto debitBalance)
        {
            var body = JsonConvert.SerializeObject(debitBalance);
            var request = HttpRequestBuilder.Create(_settings.BaseUrl,
                _settings.Endpoints.DebitBalance, HttpMethod.Post).SetRequestBody(body).Build();

            var response = await _httpClient.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                return Result.Success();
            }
            else
            {
                throw new Exception("Some thing went wrong");//TODO: change
            }
        }
    }
}
