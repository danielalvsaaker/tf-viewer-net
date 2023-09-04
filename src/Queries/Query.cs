using Infrastructure;
using Core;
using HotChocolate.Types;

namespace Queries;

public class Query
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    public IQueryable<User> Users(
        ApplicationDbContext context)
    {
        return context.Users;
    }

    [UseSingleOrDefault]
    [UseProjection]
    public IQueryable<User?> User(
        string userId,
        ApplicationDbContext context)
    {
        return context.Users
            .Where(user => user.Id == userId);
    }
}