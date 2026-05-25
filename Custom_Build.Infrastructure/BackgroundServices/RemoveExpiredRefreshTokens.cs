using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.BackgroundServices
{
    public class RemoveExpiredRefreshTokens : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RemoveExpiredRefreshTokens> _logger;
        private readonly TimeSpan _interval;

        public RemoveExpiredRefreshTokens(
            IServiceScopeFactory scopeFactory,
            ILogger<RemoveExpiredRefreshTokens> logger,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _interval = TimeSpan.FromHours(
                configuration.GetValue<int>("BackgroundServices:RefreshTokenCleanupIntervalHours"));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupAsync();
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task CleanupAsync()
        {

            using var scope = _scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var deleted = await _dbContext.RefreshTokens
                .Where(t => t.ExpierDate < DateTime.UtcNow)
                .ExecuteDeleteAsync();

            _logger.LogInformation("Cleaned {0} expired refresh tokens at {1}",
                deleted, DateTime.UtcNow);
        }
    }
}