using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class RotationalSpeedTypeInterceptor : QuantityTypeInterceptor<RotationalSpeed, RotationalSpeedUnit>
{
    public RotationalSpeedTypeInterceptor() : base(RotationalSpeedUnit.RevolutionPerMinute) {}
}