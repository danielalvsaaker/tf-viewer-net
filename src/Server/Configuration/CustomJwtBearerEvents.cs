using System.IdentityModel.Tokens.Jwt;
using Core;
using IdentityModel;
using IdentityModel.Client;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Server.Configuration;

public static class CustomJwtBearerEvents
{
    public static async Task OnTokenValidated(TokenValidatedContext validationContext)
    {
        var context = validationContext
            .HttpContext
            .RequestServices
            .GetRequiredService<ApplicationDbContext>();

        var token = (validationContext.SecurityToken as JwtSecurityToken)!;

        var cache = validationContext
            .HttpContext
            .RequestServices
            .GetRequiredService<IMemoryCache>();

        var exists = cache.Get(token.Subject);

        if (exists is not null) return;

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
            Token = token.RawData
        });

        if (await context.Users.SingleOrDefaultAsync(user => user.Id == token.Subject) is { } user)
        {
            user.Picture = new Uri(userInfo.Claims.Single(claim => claim.Type is JwtClaimTypes.Picture).Value);
        }
        else
        {
            user = new User
            {
                Id = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Subject).Value,
                Name = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Name).Value,
                Username = userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.PreferredUserName).Value,
                Picture = new Uri(userInfo.Claims.Single(claim => claim.Type == JwtClaimTypes.Picture).Value)
            };

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();

        cache.Set(token.Subject, user, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
    }
}