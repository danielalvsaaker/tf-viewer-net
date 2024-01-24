using Infrastructure;
using Core;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace Queries;

[Authorize]
[QueryType]
public class Query
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    public IQueryable<User> Users(
        ApplicationDbContext context)
    {
        return context
            .Users
            .AsNoTracking();
    }

    [UseSingleOrDefault]
    [UseProjection]
    public IQueryable<User?> User(
        string userId,
        ApplicationDbContext context)
    {
        return context
            .Users
            .AsNoTracking()
            .Where(user => user.UserId == userId);
    }
}