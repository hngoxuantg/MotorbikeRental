using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.BackgroundJobs
{
    public class DiscountCleanupService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DiscountCleanupService> logger;
        public DiscountCleanupService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            logger = serviceProvider.GetRequiredService<ILogger<DiscountCleanupService>>();
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (IServiceScope scope = serviceProvider.CreateScope())
                    {
                        DateTime now = DateTime.UtcNow;
                        IDiscountRepository discountRepository = scope.ServiceProvider.GetRequiredService<IDiscountRepository>();
                        List<Discount> discounts = (await discountRepository.GetExpiredDiscounts(cancellationToken)).ToList();
                        for (int i = 0; i < discounts.Count; i++)
                        {
                            discounts[i].IsActive = false;
                        }
                        if (discounts.Count > 0)
                            await discountRepository.SaveChangeAsync(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while cleaning up expired discounts.");
                }
                await Task.Delay(TimeSpan.FromMinutes(2));
            }
        }
    }
}
