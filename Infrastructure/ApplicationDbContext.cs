using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Activity> Activities => Set<Activity>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
}