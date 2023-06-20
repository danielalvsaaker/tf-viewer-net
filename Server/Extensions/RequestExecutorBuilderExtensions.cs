using HotChocolate.Execution.Configuration;
using Infrastructure;
using Queries;
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
            .BindRuntimeType<Speed, FloatType>()
            .TryAddTypeInterceptor<SpeedTypeInterceptor>()

            .AddEnumType<LengthUnit>()
            .BindRuntimeType<Length, FloatType>()
            .TryAddTypeInterceptor<LengthTypeInterceptor>()

            .AddEnumType<FrequencyUnit>()
            .BindRuntimeType<Frequency, FloatType>()
            .TryAddTypeInterceptor<FrequencyTypeInterceptor>()

            .AddEnumType<EnergyUnit>()
            .BindRuntimeType<Energy, FloatType>()
            .TryAddTypeInterceptor<EnergyTypeInterceptor>()

            .AddEnumType<RotationalSpeedUnit>()
            .BindRuntimeType<RotationalSpeed, FloatType>()
            .TryAddTypeInterceptor<RotationalSpeedTypeInterceptor>()

            .AddEnumType<DurationUnit>()
            .BindRuntimeType<Duration, FloatType>()
            .TryAddTypeInterceptor<DurationTypeInterceptor>()

            .AddEnumType<PowerUnit>()
            .BindRuntimeType<Power, FloatType>()
            .TryAddTypeInterceptor<PowerTypeInterceptor>();

        return services;
    }
}