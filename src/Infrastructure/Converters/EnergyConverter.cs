using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

internal class EnergyConverter() : ValueConverter<Energy, uint>(
    value => Convert.ToUInt32(value.Kilocalories),
    value => Energy.FromKilocalories(value));