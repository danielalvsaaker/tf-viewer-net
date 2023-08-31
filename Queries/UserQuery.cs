using Core;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure;

namespace Queries;

[ExtendObjectType<User>]
public class UserQuery
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    public IQueryable<Activity> Activities(
        [Parent] User parent,
        ApplicationDbContext context)
    {
        return context
            .Activities
            .Where(activity => activity.UserId == parent.Id)
            .OrderByDescending(activity => activity.Timestamp);
    }

    [UseSingleOrDefault]
    [UseProjection]
    public IQueryable<Activity?> Activity(
        [Parent] User parent,
        string activityId,
        ApplicationDbContext context)
    {
        return context
            .Activities
            .Where(activity => activity.UserId == parent.Id)
            .Where(activity => activity.ActivityId == activityId);
    }
}