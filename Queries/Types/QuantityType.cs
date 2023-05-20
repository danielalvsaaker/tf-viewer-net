using HotChocolate.Types;
using UnitsNet;

namespace Queries.Types;

public class QuantityType<TQuantity, TUnit> : ObjectType<TQuantity>
where TQuantity : IQuantity<TUnit>
where TUnit : Enum
{
    private readonly string _name;

    protected QuantityType(string name)
    {
        _name = name;
    }

    protected override void Configure(IObjectTypeDescriptor<TQuantity> descriptor)
    {
        descriptor
            .Name(_name)
            .BindFieldsExplicitly()
            .Field("value")
            .Argument("unit", arg => arg.Type<NonNullType<EnumType<TUnit>>>())
            .Resolve(ctx =>
            {
                var quantity = ctx.Parent<TQuantity>();
                var unit = ctx.ArgumentValue<TUnit>("unit");

                return quantity.As(unit);
            });
    }
}