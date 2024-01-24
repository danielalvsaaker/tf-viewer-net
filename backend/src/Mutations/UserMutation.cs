using System.Security.Claims;
using Core;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Mutations;

[Authorize]
[MutationType]
public class UserMutation
{
    [UseMutationConvention]
    public async Task<User?> FollowUser(
        string userId,
        ClaimsPrincipal principal,
        ApplicationDbContext context)
    {
        var ownerId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var owner = await context
            .Users
            .Where(user => user.UserId == ownerId)
            .Include(user => user.Following)
            .FirstAsync();

        var target = await context
            .Users
            .Where(user => user.UserId == userId)
            .FirstOrDefaultAsync();

        if (target is null)
        {
            return null;
        }

        owner.Following.Add(target);
        await context.SaveChangesAsync();

        return target;
    }

    [UseMutationConvention]
    public async Task<User?> UnfollowUser(
        string userId,
        ClaimsPrincipal principal,
        ApplicationDbContext context)
    {
        var ownerId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var owner = await context
            .Users
            .Where(user => user.UserId == ownerId)
            .Include(user => user.Following)
            .FirstAsync();

        var target = await context
            .Users
            .Where(user => user.UserId == userId)
            .FirstOrDefaultAsync();

        if (target is null)
        {
            return null;
        }

        owner.Following.Remove(target);
        await context.SaveChangesAsync();

        return target;
    }
}