using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using UnitsNet;

namespace Queries.TypeInterceptors;

public abstract class QuantityTypeInterceptor<TQuantity, TUnit> : TypeInterceptor
    where TQuantity : struct, IQuantity<TUnit>
    where TUnit : struct, Enum
{
    private readonly TUnit _defaultUnit;

    protected QuantityTypeInterceptor(TUnit defaultUnit)
    {
        _defaultUnit = defaultUnit;
    }

    public override void OnBeforeCompleteType(ITypeCompletionContext completionContext, DefinitionBase definition)
    {
        if (definition is not ObjectTypeDefinition objectTypeDefinition) return;

        foreach (var field in objectTypeDefinition.Fields)
        {
            if (!typeof(TQuantity?).IsAssignableFrom(field.ResultType))
            {
                continue;
            }

            var argument = ArgumentDescriptor.New(completionContext.DescriptorContext, "unit")
                .DefaultValue(_defaultUnit)
                .Type<NonNullType<EnumType<TUnit>>>();

            field.Arguments.Add(argument.ToDefinition());

            field
                .ToDescriptor(completionContext.DescriptorContext)
                .Extend()
                .OnBeforeCompletion((_, fieldDefinition) =>
                {
                    fieldDefinition.MiddlewareDefinitions.Add(new FieldMiddlewareDefinition(next => async context =>
                    {
                        await next(context);

                        if (context.Result is not TQuantity result)
                        {
                            return;
                        }

                        context.Result = result.As(context.ArgumentValue<TUnit>("unit"));
                    }));
                });
        }
    }
}