using CryptoMarket.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoMarket.Services
{
    public class CoinSyncBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CoinSyncBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var coinService = scope.ServiceProvider
                    .GetRequiredService<ICoinGeckoService>();
                try
                {
                    await coinService.SyncCoinsAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                ;
                await Task.Delay(
                    TimeSpan.FromMinutes(5),
                    stoppingToken);
            }
        }
    }
}