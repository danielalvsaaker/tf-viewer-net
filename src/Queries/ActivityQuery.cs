using Core;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

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
            .AsNoTracking()
            .Where(activity => activity.UserId == parent.UserId)
            .Where(activity => activity.StartTime < parent.StartTime)
            .OrderByDescending(activity => activity.StartTime);
    }

    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<Activity> Next(
        [Parent] Activity parent,
        ApplicationDbContext context)
    {
        return context
            .Activities
            .AsNoTracking()
            .Where(activity => activity.UserId == parent.UserId)
            .Where(activity => activity.StartTime > parent.StartTime)
            .OrderBy(activity => activity.StartTime);
    }

    [UseProjection]
    public IQueryable<Session> Sessions(
        [Parent] Activity parent,
        ApplicationDbContext context)
    {
        return context
            .Activities
            .AsNoTracking()
            .Where(activity => activity.UserId == parent.UserId)
            .Where(activity => activity.ActivityId == parent.ActivityId)
            .SelectMany(activity => activity.Sessions);
    }

    [UseProjection]
    public IQueryable<Record> Records(
        [Parent] Activity parent,
        ApplicationDbContext context)
    {
        return context
            .Activities
            .AsNoTracking()
            .Where(activity => activity.UserId == parent.UserId)
            .Where(activity => activity.ActivityId == parent.ActivityId)
            .SelectMany(activity => activity.Records);
    }

    [UseProjection]
    public IQueryable<Lap> Laps(
        [Parent] Activity parent,
        ApplicationDbContext context)
    {
        return context
            .Activities
            .AsNoTracking()
            .Where(activity => activity.UserId == parent.UserId)
            .Where(activity => activity.ActivityId == parent.ActivityId)
            .SelectMany(activity => activity.Laps);
    }

    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<User> Owner(
        [Parent] Activity parent,
        ApplicationDbContext context)
    {
        return context
            .Users
            .AsNoTracking()
            .Where(user => user.Id == parent.UserId);
    }
}