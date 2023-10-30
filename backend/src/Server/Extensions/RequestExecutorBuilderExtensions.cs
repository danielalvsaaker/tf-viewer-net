using HotChocolate.Execution.Configuration;
using Queries.TypeInterceptors;
using Queries.Types;
using UnitsNet;
using UnitsNet.Units;

namespace Server.Extensions;

public static class RequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddUnitTypes(this IRequestExecutorBuilder services)
    {
        services
            .AddEnumType<SpeedUnit>()
            .BindRuntimeType<Speed, SpeedType>()
            .TryAddTypeInterceptor<SpeedTypeInterceptor>()
            .AddEnumType<LengthUnit>()
            .BindRuntimeType<Length, LengthType>()
            .TryAddTypeInterceptor<LengthTypeInterceptor>()
            .AddEnumType<FrequencyUnit>()
            .BindRuntimeType<Frequency, FrequencyType>()
            .TryAddTypeInterceptor<FrequencyTypeInterceptor>()
            .AddEnumType<EnergyUnit>()
            .BindRuntimeType<Energy, EnergyType>()
            .TryAddTypeInterceptor<EnergyTypeInterceptor>()
            .AddEnumType<RotationalSpeedUnit>()
            .BindRuntimeType<RotationalSpeed, RotationalSpeedType>()
            .TryAddTypeInterceptor<RotationalSpeedTypeInterceptor>()
            .AddEnumType<DurationUnit>()
            .BindRuntimeType<Duration, DurationType>()
            .TryAddTypeInterceptor<DurationTypeInterceptor>()
            .AddEnumType<PowerUnit>()
            .BindRuntimeType<Power, PowerType>()
            .TryAddTypeInterceptor<PowerTypeInterceptor>();

        return services;
    }
}