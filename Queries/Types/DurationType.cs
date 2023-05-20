using UnitsNet;
using UnitsNet.Units;

namespace Queries.Types;

public class DurationType : QuantityType<IQuantity<DurationUnit>, DurationUnit>
{
    public DurationType() : base("Duration")
    {
    }
}