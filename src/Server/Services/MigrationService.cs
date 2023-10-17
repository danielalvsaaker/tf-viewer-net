using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Server.Services;

public class MigrationService(IDbContextFactory<ApplicationDbContext> contextFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        await context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken token) => Task.CompletedTask;
}