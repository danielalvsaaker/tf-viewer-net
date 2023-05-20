using UnitsNet;
using UnitsNet.Units;

namespace Queries.Types;

public class PowerType : QuantityType<IQuantity<PowerUnit>, PowerUnit>
{
    public PowerType() : base("Power")
    {
    }
}