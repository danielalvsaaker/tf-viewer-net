using System.Security.Claims;
using Core;
using HotChocolate.Types;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Mutations.Exceptions;
using Parser.Fit;

namespace Mutations;

[ExtendObjectType<Mutation>]
public class ActivityMutation
{
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
        ClaimsPrincipal principal,
        ApplicationDbContext context)
    {
        var ownerId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (ownerId != userId)
        {
            throw new UnauthorizedException();
        }
        
        var activity = await context.Activities
            .Where(activity => activity.UserId == userId)
            .Where(activity => activity.ActivityId == activityId)
            .FirstOrDefaultAsync();

        if (activity is not null)
        {
            context.Activities.Remove(activity);
            await context.SaveChangesAsync();
        }

        return activity?.ActivityId;
    }
}