using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

internal class LengthConverter() : ValueConverter<Length, decimal>(
    value => Convert.ToDecimal(value.Meters),
    value => Length.FromMeters(value));