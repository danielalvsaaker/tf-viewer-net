namespace Core;

public class Activity
{
    public string ActivityId { get; private set; } = null!;
    public Guid UserId { get; set; }

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
    public required ICollection<Record> Records { get; init; } = new List<Record>();
    public required ICollection<Lap> Laps { get; init; } = new List<Lap>();
}