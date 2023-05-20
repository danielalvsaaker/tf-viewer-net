using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Core;
using UnitsNet;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Activity> Activities => Set<Activity>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Duration>()
            .HaveConversion<DurationConverter>();

        configurationBuilder
            .Properties<Speed>()
            .HaveConversion<SpeedConverter>();

        configurationBuilder
            .Properties<Length>()
            .HaveConversion<LengthConverter>();

        configurationBuilder
            .Properties<RotationalSpeed>()
            .HaveConversion<RotationalSpeedConverter>();

        configurationBuilder
            .Properties<Power>()
            .HaveConversion<PowerConverter>();

        configurationBuilder
            .Properties<Frequency>()
            .HaveConversion<FrequencyConverter>();

        configurationBuilder
            .Properties<Energy>()
            .HaveConversion<EnergyConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>()
            .HasKey(activity => new
            {
                activity.UserId,
                activity.ActivityId
            });

        modelBuilder.Entity<Activity>()
            .HasOne(activity => activity.Session)
            .WithOne()
            .HasForeignKey<Session>(session => new { session.UserId, session.ActivityId });

        modelBuilder.Entity<Activity>()
            .HasMany(activity => activity.Records)
            .WithOne()
            .HasForeignKey(record => new { record.UserId, record.ActivityId });

        modelBuilder.Entity<Activity>()
            .HasMany(activity => activity.Laps)
            .WithOne()
            .HasForeignKey(lap => new { lap.UserId, lap.ActivityId });

        modelBuilder.Entity<Session>()
            .HasKey(session => new
            {
                session.UserId,
                session.ActivityId
            });

        modelBuilder.Entity<Record>()
            .HasKey(record => new
            {
                record.UserId,
                record.ActivityId,
                record.Timestamp
            });

        modelBuilder.Entity<Lap>()
            .HasKey(lap => new
            {
                lap.UserId,
                lap.ActivityId,
                lap.Timestamp
            });
    }
}