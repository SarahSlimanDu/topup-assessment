using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Infrastructure.Persistence;
using TopUpBeneficiary.Infrastructure.Persistence.Commons;
using TopUpBeneficiary.Infrastructure.Persistence.Repository;

namespace TopUpBeneficiary.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                                 b => b.MigrationsAssembly("TopUpBeneficiary.Infrastructure"));
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddScoped<ITopUpTransactionRepository, TopUpTransactionRepository>();
            services.AddScoped<ITopUpOptionsRepository, TopUpOptionsRepository>();  
            services.AddScoped<IUnitOfWork, UnitOfWork>();
 
            return services;
        }
    }
}
