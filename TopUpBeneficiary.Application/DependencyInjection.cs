using Microsoft.Extensions.DependencyInjection;
using TopUpBeneficiary.Application.Services.Beneficiaries;
using TopUpBeneficiary.Application.Services.TopUp;

namespace TopUpBeneficiary.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IBeneficiaryService, BeneficiaryService>();
            services.AddScoped<ITopUpService, TopUpService>();

            return services;
        }
    }
}
