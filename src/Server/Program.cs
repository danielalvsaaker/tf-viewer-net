using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Core;
using IdentityModel;
using IdentityModel.Client;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Mutations;
using Parser.Fit;
using Queries;
using Server.Extensions;
using Server.Services;


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
            OnTokenValidated = async validationContext =>
            {
                var context = validationContext
                    .HttpContext
                    .RequestServices
                    .GetRequiredService<ApplicationDbContext>();

                var token = validationContext.SecurityToken as JwtSecurityToken;

                var cache = validationContext
                    .HttpContext
                    .RequestServices
                    .GetRequiredService<IMemoryCache>();

                var exists = cache.Get(token.Subject);

                if (exists is not null)
                {
                    return;
                }

                var client = validationContext
                    .HttpContext
                    .RequestServices
                    .GetRequiredService<IHttpClientFactory>()
                    .CreateClient();

                var openIdConnectConfiguration = await validationContext
                    .Options
                    .ConfigurationManager!
                    .GetConfigurationAsync(validationContext.HttpContext.RequestAborted);

                var userInfo = await client.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = openIdConnectConfiguration.UserInfoEndpoint,
                    Token = token.RawData,
                });

                if (await context.Users.SingleOrDefaultAsync(user => user.Id == token.Subject) is {} user)
                {
                    // Update existing user
                }
                else
                {
                    user = new User
                    {
                        Id = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Subject).Value,
                        Name = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Name).Value,
                        Username = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.PreferredUserName).Value,
                        Picture = new Uri(userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Picture).Value),
                    };

                    context.Users.Add(user);
                }

                await context.SaveChangesAsync();

                cache.Set(token.Subject, user, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            },
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