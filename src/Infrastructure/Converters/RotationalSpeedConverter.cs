using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

internal class RotationalSpeedConverter : ValueConverter<RotationalSpeed, decimal>
{
    public RotationalSpeedConverter() : base(
        value => Convert.ToDecimal(value.RevolutionsPerMinute),
        value => RotationalSpeed.FromRevolutionsPerMinute(value)
    )
    { }
}