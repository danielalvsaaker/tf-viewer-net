using Core;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

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
            .AsNoTracking()
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
            .AsNoTracking()
            .Where(activity => activity.UserId == parent.Id)
            .Where(activity => activity.ActivityId == activityId);
    }

    [UsePaging]
    [UseProjection]
    public IQueryable<User> Following(
        [Parent] User parent,
        ApplicationDbContext context)
    {
        return context
            .Users
            .AsNoTracking()
            .Where(user => user.Id == parent.Id)
            .SelectMany(user => user.Following);
    }

    [UsePaging]
    [UseProjection]
    public IQueryable<User> Followers(
        [Parent] User parent,
        ApplicationDbContext context)
    {
        return context
            .Users
            .AsNoTracking()
            .Where(user => user.Id == parent.Id)
            .SelectMany(user => user.Followers);
    }
}