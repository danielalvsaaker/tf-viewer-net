namespace Core;

public class User
{
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string Name { get; init; }
    public Uri? Picture { get; init; }

    public IEnumerable<Activity> Activities { get; set; } = new List<Activity>();
}