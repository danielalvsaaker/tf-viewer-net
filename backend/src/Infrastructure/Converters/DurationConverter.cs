using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

internal class DurationConverter() : ValueConverter<Duration, decimal>(
    value => Convert.ToDecimal(value.Seconds),
    value => Duration.FromSeconds(value));