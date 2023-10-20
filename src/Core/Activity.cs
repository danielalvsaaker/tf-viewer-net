using HotChocolate;
using NetTopologySuite.Geometries;
using UnitsNet;

namespace Core;

public class Activity
{
    [IsProjected(true)]
    public string UserId { get; set; } = null!;
    [IsProjected(true)]
    public string ActivityId { get; private set; } = null!;
    
    [IsProjected(true)]
    public required DateTimeOffset StartTime
    {
        get => _startTime;
        init
        {
            _startTime = value;
            ActivityId = _startTime.ToString("yyyyMMddHHmmss");
        }
    }
    private readonly DateTimeOffset _startTime;

    public Duration TotalTimerTime { get; set; }
    public required Geometry BoundingBox { get; set; }

    [GraphQLIgnore]
    public required ICollection<Session> Sessions { get; init; } = new List<Session>();
    [GraphQLIgnore]
    public required ICollection<Record> Records { get; init; } = new List<Record>();
    [GraphQLIgnore]
    public required ICollection<Lap> Laps { get; init; } = new List<Lap>();
}