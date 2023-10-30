using HotChocolate.Types.Spatial;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Mutations;
using Mutations.ErrorFilters;
using NetTopologySuite.Geometries;
using Parser.Fit;
using Queries;
using Server.Configuration;
using Server.Extensions;
using Server.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPooledDbContextFactory<ApplicationDbContext>(options =>
    {
        if (builder.Configuration.GetConnectionString(Infrastructure.Postgres.Provider.Name) is { } connectionString)
        {
            options.UseNpgsql(
                connectionString,
                options => options
                    .UseNetTopologySuite()
                    .MigrationsAssembly(Infrastructure.Postgres.Provider.Assembly));
        }
        else
        {
            options.UseSqlite(
                "Data Source=tf.db",
                options => options
                    .UseNetTopologySuite()
                    .MigrationsAssembly(Infrastructure.Sqlite.Provider.Assembly));
        }
    });

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddSpatialFiltering()
    .BindRuntimeType<Geometry, GeoJsonInterfaceType>()
    .AddSpatialTypes()
    .AddSpatialProjections()
    .AddUnitTypes()
    .AddQueryType<Query>()
    .AddType<UploadType>()
    .AddTypeExtension<ActivityQuery>()
    .AddTypeExtension<UserQuery>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<UserMutation>()
    .AddTypeExtension<ActivityMutation>()
    .AddMutationConventions()
    .RegisterService<ActivityParser>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .TrimTypes()
    .AddErrorFilter<UnauthorizedErrorFilter>()
    .InitializeOnStartup();

builder.Services
    .AddHttpClient()
    .AddMemoryCache();

builder.Services
    .AddScoped<ActivityParser>();

builder.Services.AddHostedService<MigrationService>();

builder.Services
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = CustomJwtBearerEvents.OnTokenValidated
        };

        builder
            .Configuration
            .GetSection("JWT")
            .Bind(options);
        options.Validate();
    });

var app = builder.Build();

app.UseAuthentication()
    .UseAuthorization();

app.MapGraphQL();

app.Run();