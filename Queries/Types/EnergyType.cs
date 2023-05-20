using UnitsNet;
using UnitsNet.Units;

namespace Queries.Types;

public class EnergyType : QuantityType<IQuantity<EnergyUnit>, EnergyUnit>
{
    public EnergyType() : base("Energy")
    {
    }
}