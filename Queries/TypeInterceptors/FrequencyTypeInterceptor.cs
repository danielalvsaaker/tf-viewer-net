using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class FrequencyTypeInterceptor : QuantityTypeInterceptor<Frequency, FrequencyUnit>
{
    public FrequencyTypeInterceptor() : base(FrequencyUnit.BeatPerMinute) {}
}