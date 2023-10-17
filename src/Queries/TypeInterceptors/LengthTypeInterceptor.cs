using UnitsNet;
using UnitsNet.Units;

namespace Queries.TypeInterceptors;

public class LengthTypeInterceptor() : QuantityTypeInterceptor<Length, LengthUnit>(LengthUnit.Meter);