using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

internal class SpeedConverter : ValueConverter<Speed, decimal>
{
    public SpeedConverter() : base(
        value => Convert.ToDecimal(value.MetersPerSecond),
        value => Speed.FromMetersPerSecond(value))
    { }
}