using Infrastructure;
using Core;

namespace Queries;

public class Query
{
    [UseProjection]
    public IQueryable<Activity?> Activities(
        ApplicationDbContext context)
    {
        return context.Activities
            .AsQueryable();
    }
}