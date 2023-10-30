using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class DurationTypeInterceptor() : QuantityTypeInterceptor<Duration, DurationUnit>(DurationUnit.Second);