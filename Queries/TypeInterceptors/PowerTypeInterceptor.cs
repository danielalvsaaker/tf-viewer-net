using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class PowerTypeInterceptor : QuantityTypeInterceptor<Power, PowerUnit>
{
    public PowerTypeInterceptor() : base(PowerUnit.Watt) {}
}