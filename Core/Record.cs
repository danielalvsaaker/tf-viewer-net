using NetTopologySuite.Geometries;
using UnitsNet;

namespace Core;

public class Record
{
    public string ActivityId { get; private set; } = null!;
    public Guid UserId { get; private set; }

    public required DateTimeOffset Timestamp { get; init; }

    public Point? Position { get; set; }

    public RotationalSpeed? Cadence { get; init; }
    public Length? Distance { get; init; }
    public Length? Altitude { get; init; }
    public Speed? Speed { get; init; }
    public Frequency? HeartRate { get; init; }
    public Power? Power { get; init; }
}