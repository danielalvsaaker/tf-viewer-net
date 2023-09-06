using System.Security.Claims;
using Core;
using HotChocolate.Authorization;
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
}