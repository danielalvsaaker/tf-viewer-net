using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class EnergyTypeInterceptor : QuantityTypeInterceptor<Energy, EnergyUnit>
{
    public EnergyTypeInterceptor() : base(EnergyUnit.Kilocalorie) {}
}