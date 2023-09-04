using HotChocolate.Types;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Mutations;

public class Mutation
{
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