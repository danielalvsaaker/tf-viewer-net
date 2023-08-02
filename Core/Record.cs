using NetTopologySuite.Geometries;
using UnitsNet;

namespace Core;

public class Record
{
    public string ActivityId { get; private set; } = null!;
    public string UserId { get; private set; }

    public required DateTimeOffset Timestamp { get; init; }

    public required Point? Position { get; set; }

    public required RotationalSpeed? Cadence { get; set; }
    public required Length? Distance { get; set; }
    public required Length? Altitude { get; set; }
    public required Speed? Speed { get; set; }
    public required Frequency? HeartRate { get; set; }
    public required Power? Power { get; set; }
}