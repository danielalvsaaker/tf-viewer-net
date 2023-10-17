using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

internal class SpeedConverter() : ValueConverter<Speed, decimal>(
    value => Convert.ToDecimal(value.MetersPerSecond),
    value => Speed.FromMetersPerSecond(value));