using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser.Fit;
using Queries;
using Server.Extensions;

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
    .AddSpatialProjections()
    .AddUnitTypes()

    .AddQueryType<Query>()
    .AddTypeExtension<ActivityQuery>()

    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .TrimTypes()
    .InitializeOnStartup();

builder.Services
    .AddScoped<ActivityParser>();

builder.Services
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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