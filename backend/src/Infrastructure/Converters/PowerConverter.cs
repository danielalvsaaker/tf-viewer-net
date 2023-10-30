using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

internal class PowerConverter() : ValueConverter<Power, decimal>(
    value => Convert.ToDecimal(value.Watts),
    value => Power.FromWatts(value));