using Queries.Types;
using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class DurationTypeInterceptor : QuantityTypeInterceptor<Duration, DurationUnit>
{
    public DurationTypeInterceptor() : base(DurationUnit.Second) {}
}