using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class RotationalSpeedTypeInterceptor() : QuantityTypeInterceptor<RotationalSpeed, RotationalSpeedUnit>(RotationalSpeedUnit.RevolutionPerMinute);