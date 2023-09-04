using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

public class EnergyConverter : ValueConverter<Energy, uint>
{
    public EnergyConverter() : base(
        value => Convert.ToUInt32(value.Kilocalories),
        value => Energy.FromKilocalories(value))
    { }
}