using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Core;
using IdentityModel;
using IdentityModel.Client;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO.Converters;
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
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async (ctx) =>
            {
                var context = ctx.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
                var token = ctx.SecurityToken as JwtSecurityToken;

                if (context.Users.Any(user => user.Id == token.Subject))
                {
                    return;
                }

                var client = new HttpClient();

                var discovery = await client.GetDiscoveryDocumentAsync(ctx.Options.MetadataAddress);
                var userInfo = await client.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = discovery.UserInfoEndpoint,
                    Token = token.RawData,
                });

                var user = new User
                {
                    Id = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Subject).Value,
                    Name = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Name).Value,
                    Username = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.PreferredUserName).Value,
                    Picture = new Uri(userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Picture).Value),
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
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