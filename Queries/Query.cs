using Infrastructure;
using Core;
using HotChocolate.Types;

namespace Queries;

public class Query
{
    [UsePaging]
    [UseProjection]
    public IQueryable<Activity?> Activities(
        ApplicationDbContext context)
    {
        return context.Activities;
    }
}