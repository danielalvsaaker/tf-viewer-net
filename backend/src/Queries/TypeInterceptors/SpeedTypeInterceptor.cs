using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class SpeedTypeInterceptor() : QuantityTypeInterceptor<Speed, SpeedUnit>(SpeedUnit.MeterPerSecond);