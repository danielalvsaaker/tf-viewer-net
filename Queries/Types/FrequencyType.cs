using UnitsNet;
using UnitsNet.Units;

namespace Queries.Types;

public class FrequencyType : QuantityType<IQuantity<FrequencyUnit>, FrequencyUnit>
{
    public FrequencyType() : base("Frequency")
    {
    }
}