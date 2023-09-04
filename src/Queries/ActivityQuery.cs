using Core;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure;

namespace Queries;

[ExtendObjectType<Activity>]
public class ActivityQuery
{
    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<Activity> Previous(
        [Parent] Activity parent,
        ApplicationDbContext context)
    {
        return context
            .Activities
            .Where(activity => activity.UserId == parent.UserId)
            .Where(activity => activity.Timestamp < parent.Timestamp)
            .OrderByDescending(activity => activity.Timestamp);
    }

    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<Activity> Next(
        [Parent] Activity parent,
        ApplicationDbContext context)
    {
        return context
            .Activities
            .Where(activity => activity.UserId == parent.UserId)
            .Where(activity => activity.Timestamp > parent.Timestamp)
            .OrderBy(activity => activity.Timestamp);
    }
}