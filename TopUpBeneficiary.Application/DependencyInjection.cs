using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TopUpBeneficiary.Application.Services.Beneficiaries;
using TopUpBeneficiary.Application.Services.TopUp;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Application.SyncDataService.WebService.Settings;

namespace TopUpBeneficiary.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBeneficiaryService, BeneficiaryService>();
            services.AddScoped<ITopUpService, TopUpService>();

            services.Configure<AccountServiceSettings>(configuration.GetSection(nameof(AccountServiceSettings)));

            services.AddHttpClient<IAccountClient, AccountClient>();

            return services;
        }
    }
}
