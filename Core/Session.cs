using NetTopologySuite.Geometries;
using UnitsNet;

namespace Core;

public class Session
{
    public string ActivityId { get; private set; } = null!;
    public Guid UserId { get; private set; }

    public Point? NorthEastCorner { get; init; }
    public Point? SouthWestCorner { get; init; }

    public Duration Duration { get; init; }
    public Duration DurationActive { get; init; }

    public Length? Distance { get; set; }

    public RotationalSpeed? CadenceAverage { get; init; }
    public RotationalSpeed? CadenceMax { get; init; }

    public Frequency? HeartRateAverage { get; init; }
    public Frequency? HeartRateMax { get; init; }

    public Speed? SpeedAverage { get; init; }
    public Speed? SpeedMax { get; init; }

    public Power PowerAverage { get; init; }
    public Power PowerMax { get; init; }

    public Length? Ascent { get; init; }
    public Length? Descent { get; init; }

    public Energy? Calories { get; init; }
}