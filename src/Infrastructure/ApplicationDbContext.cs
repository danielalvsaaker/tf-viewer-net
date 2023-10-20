using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Core;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Activity> Activities => Set<Activity>();

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Activity>()
            .HasKey(activity => new
            {
                activity.UserId,
                activity.ActivityId
            });

        builder.Entity<Activity>()
            .HasMany(activity => activity.Sessions)
            .WithOne()
            .HasForeignKey(session => new { session.UserId, session.ActivityId });

        builder.Entity<Activity>()
            .HasMany(activity => activity.Records)
            .WithOne()
            .HasForeignKey(record => new { record.UserId, record.ActivityId });

        builder.Entity<Activity>()
            .HasMany(activity => activity.Laps)
            .WithOne()
            .HasForeignKey(lap => new { lap.UserId, lap.ActivityId });

        builder.Entity<Session>()
            .HasKey(session => new
            {
                session.UserId,
                session.ActivityId,
                session.StartTime
            });

        builder.Entity<Record>()
            .HasKey(record => new
            {
                record.UserId,
                record.ActivityId,
                record.Timestamp
            });

        builder.Entity<Lap>()
            .HasKey(lap => new
            {
                lap.UserId,
                lap.ActivityId,
                lap.StartTime
            });

        builder.Entity<User>()
            .HasMany(user => user.Followers)
            .WithMany(user => user.Following)
            .UsingEntity(join => join.ToTable("Followers"));

        if (Database.IsSqlite())
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var properties = entityType
                    .ClrType
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

                foreach (var property in properties)
                {
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }
        
        base.OnModelCreating(builder);
    }
}