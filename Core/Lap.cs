using NetTopologySuite.Geometries;
using UnitsNet;

namespace Core;

public class Lap
{
    public string ActivityId { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public required DateTimeOffset Timestamp { get; init; }

    public Point? StartPosition { get; init; }
    public Point? EndPosition { get; init; }

    public Duration Duration { get; init; }
    public Duration DurationActive { get; init; }

    public Length? Distance { get; init; }

    public RotationalSpeed? CadenceAverage { get; init; }
    public RotationalSpeed? CadenceMax { get; init; }

    public Frequency? HeartRateAverage { get; init; }
    public Frequency? HeartRateMax { get; init; }

    public Speed? SpeedAverage { get; init; }
    public Speed? SpeedMax { get; init; }

    public Power? PowerAverage { get; init; }
    public Power? PowerMax { get; init; }

    public Length? Ascent { get; init; }
    public Length? Descent { get; init; }

    public Energy? Calories { get; init; }
}