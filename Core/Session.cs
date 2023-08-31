using HotChocolate;
using NetTopologySuite.Geometries;
using UnitsNet;

namespace Core;

public class Session
{
    [GraphQLIgnore]
    public string ActivityId { get; private set; } = null!;
    [GraphQLIgnore]
    public string UserId { get; private set; }

    public required DateTimeOffset StartTime { get; init; }

    public required Point? NorthEastCorner { get; set; }
    public required Point? SouthWestCorner { get; set; }

    public required Duration Duration { get; set; }
    public required Duration DurationActive { get; set; }

    public required Length? Distance { get; set; }

    public required RotationalSpeed? CadenceAverage { get; set; }
    public required RotationalSpeed? CadenceMax { get; set; }

    public required Frequency? HeartRateAverage { get; set; }
    public required Frequency? HeartRateMax { get; set; }

    public required Speed? SpeedAverage { get; init; }
    public required Speed? SpeedMax { get; init; }

    public required Power? PowerAverage { get; init; }
    public required Power? PowerMax { get; init; }

    public required Length? Ascent { get; init; }
    public required Length? Descent { get; init; }

    public required Energy? Calories { get; init; }
}