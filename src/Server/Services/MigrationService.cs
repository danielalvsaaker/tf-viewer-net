using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Server.Services;

public class MigrationService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MigrationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        var _context = scope
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await _context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken token) => Task.CompletedTask;
}
