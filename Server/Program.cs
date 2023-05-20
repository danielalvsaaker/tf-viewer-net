using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Queries;
using Queries.Types;
using UnitsNet;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlite(
            "Data Source=tf.db",
            x => x.UseNetTopologySuite());
    });

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<ApplicationDbContext>()
    .AddProjections()
    .AddSpatialTypes()
    .AddQueryType<Query>()
    .BindRuntimeType<Length, LengthType>()
    .BindRuntimeType<Speed, SpeedType>()
    .BindRuntimeType<Frequency, FrequencyType>()
    .BindRuntimeType<Energy, EnergyType>()
    .BindRuntimeType<RotationalSpeed, RotationalSpeedType>()
    .BindRuntimeType<Duration, DurationType>()
    .BindRuntimeType<Power, PowerType>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .InitializeOnStartup();

var app = builder.Build();

app.MapGraphQL();

app.Run();