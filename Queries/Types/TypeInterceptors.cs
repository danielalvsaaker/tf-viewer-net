using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using UnitsNet;
using UnitsNet.Units;

namespace Queries.Types;

public class UnitTypeInterceptor<T, TUnit> : TypeInterceptor
where T : struct, IQuantity<TUnit>
where TUnit : struct, Enum
{
    public override void OnBeforeCompleteType(ITypeCompletionContext completionContext, DefinitionBase definition)
    {
        if (definition is not ObjectTypeDefinition objectTypeDefinition) return;

        foreach (var field in objectTypeDefinition.Fields)
        {
            if (typeof(T?).IsAssignableFrom(field.ResultType))
            {
                var argument = ArgumentDescriptor.New(completionContext.DescriptorContext, "unit")
                    .Type<EnumType<TUnit>>();
                field.Arguments.Add(argument.ToDefinition());

                field
                    .ToDescriptor(completionContext.DescriptorContext)
                    .Extend()
                    .OnBeforeCompletion((_, fieldDefinition) =>
                    {
                        fieldDefinition.MiddlewareDefinitions.Add(new FieldMiddlewareDefinition(next => async context =>
                        {
                            await next(context);

                            if (context.Result is not T result)
                            {
                                return;
                            };

                            context.Result = context.ArgumentValue<TUnit?>("unit") switch
                            {
                                null => result.As(result.Unit),
                                var unit => result.As(unit),
                            };
                        }));
                    });
            }
        }
    }
}

public class SpeedTypeInterceptor : UnitTypeInterceptor<Speed, SpeedUnit> { }
public class LengthTypeInterceptor : UnitTypeInterceptor<Length, LengthUnit> { }
public class FrequencyTypeInterceptor : UnitTypeInterceptor<Frequency, FrequencyUnit> { }
public class EnergyTypeInterceptor : UnitTypeInterceptor<Energy, EnergyUnit> { }
public class RotationalSpeedTypeInterceptor : UnitTypeInterceptor<RotationalSpeed, RotationalSpeedUnit> { }
public class DurationTypeInterceptor : UnitTypeInterceptor<Duration, DurationUnit> { }
public class PowerTypeInterceptor : UnitTypeInterceptor<Power, PowerUnit> { }