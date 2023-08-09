using HotChocolate.Language;
using HotChocolate.Types;

namespace Queries.Types;

public abstract class QuantityType<TQuantity> : ScalarType<decimal, FloatValueNode>
{
    protected QuantityType() : base(typeof(TQuantity).Name) {}

    public override IValueNode ParseResult(object? resultValue)
        => ParseValue(resultValue);

    protected override decimal ParseLiteral(FloatValueNode valueSyntax)
        => valueSyntax.ToDecimal();

    protected override FloatValueNode ParseValue(decimal runtimeValue) => new(runtimeValue);
}