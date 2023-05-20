using Infrastructure;
using Core;

namespace Queries;

public class Query
{
    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<Activity?> Activity(
        ApplicationDbContext context,
        Guid userId,
        string activityId)
    {
        return context.Activities
            .Where(activity => activity.UserId == userId)
            .Where(activity => activity.ActivityId == activityId);
    }
}