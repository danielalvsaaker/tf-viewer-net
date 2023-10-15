using HotChocolate;

namespace Core;

public class Activity
{
    [IsProjected(true)]
    public string ActivityId { get; private set; } = null!;
    [IsProjected(true)]
    public string UserId { get; set; }
    [IsProjected(true)]
    public required DateTimeOffset Timestamp
    {
        get => _timestamp;
        init
        {
            _timestamp = value;
            ActivityId = _timestamp.ToString("yyyyMMddHHmmss");
        }
    }
    private readonly DateTimeOffset _timestamp;

    public required Session Session { get; init; }

    [GraphQLIgnore]
    public required ICollection<Record> Records { get; init; } = new List<Record>();
    [GraphQLIgnore]
    public required ICollection<Lap> Laps { get; init; } = new List<Lap>();
}