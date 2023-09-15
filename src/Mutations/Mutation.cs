using System.Security.Claims;
using Core;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Mutations.Exceptions;
using Parser.Fit;

namespace Mutations;

public class Mutation
{
    [Authorize]
    [Error<ActivityExistsException>]
    public async Task<Activity> UploadActivity(
        IFile file,
        ClaimsPrincipal principal,
        ActivityParser parser,
        ApplicationDbContext context)
    {
        await using var stream = file.OpenReadStream();
        var uploadedActivity = parser.Parse(stream);

        uploadedActivity.UserId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        if (await context.Activities
                .Where(activity => activity.UserId == uploadedActivity.UserId)
                .AnyAsync(activity => activity.ActivityId == uploadedActivity.ActivityId))
        {
            throw new ActivityExistsException(uploadedActivity.UserId, uploadedActivity.ActivityId);
        }

        context.Activities.Add(uploadedActivity);
        await context.SaveChangesAsync();
        
        return uploadedActivity;
    }
    
    [UseMutationConvention(PayloadFieldName = nameof(activityId))]
    public async Task<string?> DeleteActivity(
        string userId,
        string activityId,
        ApplicationDbContext context)
    {
        var rows = await context.Activities
            .Where(activity => activity.UserId == userId)
            .Where(activity => activity.ActivityId == activityId)
            .ExecuteDeleteAsync();

        return rows switch
        {
            > 0 => activityId,
            _ => null,
        };
    }

    [UseFirstOrDefault]
    [UseProjection]
    [UseMutationConvention]
    public async Task<IQueryable<User>?> FollowUser(
        string userId,
        ClaimsPrincipal principal,
        ApplicationDbContext context)
    {
        var ownerId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var owner = await context
            .Users
            .Where(user => user.Id == ownerId)
            .Include(user => user.Following)
            .FirstAsync();

        var target = await context
            .Users
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();

        if (target is null)
        {
            return null;
        }

        owner.Following.Add(target);
        await context.SaveChangesAsync();

        return context
            .Users
            .AsNoTracking()
            .Where(user => user.Id == target.Id);
    }

    [UseFirstOrDefault]
    [UseProjection]
    [UseMutationConvention]
    public async Task<IQueryable<User>?> UnfollowUser(
        string userId,
        ClaimsPrincipal principal,
        ApplicationDbContext context)
    {
        var ownerId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var owner = await context
            .Users
            .Where(user => user.Id == ownerId)
            .Include(user => user.Following)
            .FirstAsync();

        var target = await context
            .Users
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();

        if (target is null)
        {
            return null;
        }
        
        owner.Following.Remove(target);
        await context.SaveChangesAsync();

        return context
            .Users
            .AsNoTracking()
            .Where(user => user.Id == target.Id);
    }
}