using HotChocolate;

namespace Core;

public class User
{
    [IsProjected(true)]
    public required string UserId { get; init; }
    public required string Username { get; init; }
    public required string Name { get; init; }
    public Uri? Picture { get; set; }

    [GraphQLIgnore]
    public IEnumerable<Activity> Activities { get; set; } = new List<Activity>();

    [GraphQLIgnore]
    public ICollection<User> Followers { get; set; } = new List<User>();
    [GraphQLIgnore]
    public ICollection<User> Following { get; set; } = new List<User>();
}