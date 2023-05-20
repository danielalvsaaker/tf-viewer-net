using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

public class DurationConverter : ValueConverter<Duration, decimal>
{
    public DurationConverter() : base(
        value => Convert.ToDecimal(value.Seconds),
        value => Duration.FromSeconds(value))
    { }
}