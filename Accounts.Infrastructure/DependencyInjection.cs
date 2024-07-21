using Accounts.Domain.Interface;
using Accounts.Infrastructure.Persistence;
using Accounts.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                                 b => b.MigrationsAssembly("Accounts.Infrastructure"));
            });

            services.AddScoped<IAccountRepository, AccountRepository>();    
            return services;
        }
    }
}
