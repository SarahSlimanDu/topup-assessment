using Accounts.Domain.AccountAggregate;
using Accounts.Domain.Interface;
using Accounts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Api
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

            #region Seed Account
            var accountRepository = serviceScope.ServiceProvider.GetRequiredService<IAccountRepository>();
            var unitOfWork = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();    
            var accounts = await accountRepository.GetAllAccounts();
            if(accounts.Count() == 0)
            {
                accountRepository.Add(Account.Create("DE89370400440532013000", "test1@account.com", "P@ssw0rd", 1000, "AED"));
                accountRepository.Add(Account.Create("DE89370400440532013111", "test2@account.com", "P@ssw0rd", 100, "AED"));
                await unitOfWork.Save();
            }
            #endregion
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

