using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Renting.Data;
using Renting.Models;

public class RentalCleanupHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RentalCleanupHostedService> _logger;

    public RentalCleanupHostedService(IServiceProvider serviceProvider, ILogger<RentalCleanupHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RentingContext>();

            var threshold = DateTime.UtcNow.AddDays(-3);
            var oldPendingRentals = await db.Rentals
                .Where(r => r.Status == Statusenum.Pending && r.FromDate < threshold)
                .ToListAsync(stoppingToken);

            foreach (var rental in oldPendingRentals)
            {
                rental.Status = Statusenum.Cancelled;
                _logger.LogInformation("Rental {RentalId} cancelled due to timeout.", rental.Id);
            }

            if (oldPendingRentals.Count > 0)
                await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // uruchamiaj co godzinê
        }
    }
}