using HotChocolate;

namespace Core;

public class User
{
    [IsProjected(true)]
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string Name { get; init; }
    public Uri? Picture { get; set; }

    [GraphQLIgnore]
    public IEnumerable<Activity> Activities { get; set; } = new List<Activity>();
}