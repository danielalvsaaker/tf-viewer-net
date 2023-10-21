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
            .OrderByDescending(activity => activity.StartTime);
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

    [UsePaging]
    [UseProjection]
    public IQueryable<Activity> Feed(
        [Parent] User parent,
        ApplicationDbContext context)
    {
        var user = context
            .Users
            .AsNoTracking()
            .Where(user => user.Id == parent.Id);

        var activities = user
            .SelectMany(user => user.Activities);
        var followingActivities = user
            .SelectMany(user => user.Following.SelectMany(following => following.Activities));

        return activities
            .Concat(followingActivities)
            .OrderByDescending(activity => activity.StartTime);

    }
}