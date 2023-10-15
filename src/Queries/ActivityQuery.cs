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
            .AsNoTracking()
            .Where(activity => activity.UserId == parent.UserId)
            .Where(activity => activity.Timestamp > parent.Timestamp)
            .OrderBy(activity => activity.Timestamp);
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
}