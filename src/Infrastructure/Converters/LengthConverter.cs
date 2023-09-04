using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

internal class LengthConverter : ValueConverter<Length, decimal>
{
    public LengthConverter() : base(
        value => Convert.ToDecimal(value.Meters),
        value => Length.FromMeters(value)
    )
    { }
}