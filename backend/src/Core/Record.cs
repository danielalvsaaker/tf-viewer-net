using HotChocolate;
using NetTopologySuite.Geometries;
using UnitsNet;

namespace Core;

public class Record
{
    [GraphQLIgnore]
    public string UserId { get; private set; } = null!;
    [GraphQLIgnore]
    public string ActivityId { get; private set; } = null!;

    public required DateTimeOffset Timestamp { get; init; }

    public required Point? Position { get; set; }

    public required RotationalSpeed? Cadence { get; set; }
    public required Length? Distance { get; set; }
    public required Length? Altitude { get; set; }
    public required Speed? Speed { get; set; }
    public required Frequency? HeartRate { get; set; }
    public required Power? Power { get; set; }
}