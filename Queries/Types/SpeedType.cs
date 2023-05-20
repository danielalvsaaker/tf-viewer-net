using UnitsNet;
using UnitsNet.Units;

namespace Queries.Types;

public class SpeedType : QuantityType<IQuantity<SpeedUnit>, SpeedUnit>
{
    public SpeedType() : base("Speed")
    {
    }
}