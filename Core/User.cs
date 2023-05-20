namespace Core;

public class User
{
    public Guid Id { get; init; }

    public IEnumerable<Activity> Activities { get; set; } = new List<Activity>();
}