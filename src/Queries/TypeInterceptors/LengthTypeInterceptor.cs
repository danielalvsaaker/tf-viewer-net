using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class LengthTypeInterceptor : QuantityTypeInterceptor<Length, LengthUnit>
{
    public LengthTypeInterceptor() : base(LengthUnit.Meter) {}
}