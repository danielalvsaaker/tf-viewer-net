using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Mutations;
using Parser.Fit;
using Queries;
using Server.Configuration;
using Server.Extensions;
using Server.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
    {
        if (builder.Configuration.GetConnectionString(Infrastructure.Postgres.Provider.Name) is {} connectionString)
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
    .RegisterDbContext<ApplicationDbContext>()
    .AddSpatialTypes()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddSpatialFiltering()
    .AddSpatialProjections()
    .AddUnitTypes()
    .AddQueryType<Query>()
    .AddTypeExtension<ActivityQuery>()
    .AddTypeExtension<UserQuery>()
    .AddMutationType<Mutation>()
    .AddMutationConventions()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .TrimTypes()
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