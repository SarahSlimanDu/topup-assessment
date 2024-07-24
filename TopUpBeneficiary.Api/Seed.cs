using Microsoft.EntityFrameworkCore;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate;
using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;
using TopUpBeneficiary.Infrastructure.Persistence;

namespace TopUpBeneficiary.Api
{
    public class Seed(IServiceProvider serviceProvider) : IHostedService
    {
        protected readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var serviceScope = this._serviceProvider.CreateAsyncScope();

            #region Context
            var appDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await appDbContext.Database.MigrateAsync(cancellationToken);
            #endregion

            #region Seed Users
            var userRepository = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
            var unitOfWork = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();    
            var users = await userRepository.GetAll();
            if(users.Count() == 0)
            {
                userRepository.Add(User.Create("test1@topup.com", true, "DE89370400440532013000"));
                userRepository.Add(User.Create("test2@topup.com", false, "DE89370400440532013111"));
                await unitOfWork.Save();
            }
            #endregion

            #region Seed TopUpOptios
            var topUpOptionRepository = serviceScope.ServiceProvider.GetRequiredService<ITopUpOptionsRepository>();
            var topUpOptions = await topUpOptionRepository.GetAll();    
            if(topUpOptions.Count() == 0)
            {
                topUpOptionRepository.Add(TopUpOption.Create(5));
                topUpOptionRepository.Add(TopUpOption.Create(10));
                topUpOptionRepository.Add(TopUpOption.Create(20));
                topUpOptionRepository.Add(TopUpOption.Create(30));
                topUpOptionRepository.Add(TopUpOption.Create(50));
                topUpOptionRepository.Add(TopUpOption.Create(75));
                topUpOptionRepository.Add(TopUpOption.Create(100));
                await unitOfWork.Save();
            }
            #endregion
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
