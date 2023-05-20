using UnitsNet;
using UnitsNet.Units;

namespace Queries.Types;

public class RotationalSpeedType : QuantityType<IQuantity<RotationalSpeedUnit>, RotationalSpeedUnit>
{
    public RotationalSpeedType() : base("RotationalSpeed")
    {
    }
}