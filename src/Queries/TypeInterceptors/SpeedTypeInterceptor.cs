using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class SpeedTypeInterceptor : QuantityTypeInterceptor<Speed, SpeedUnit>
{
    public SpeedTypeInterceptor() : base(SpeedUnit.MeterPerSecond) {}
}