using UnitsNet;
using UnitsNet.Units;

namespace Queries.Types;

public class LengthType : QuantityType<IQuantity<LengthUnit>, LengthUnit>
{
    public LengthType() : base("Length")
    {
    }
}