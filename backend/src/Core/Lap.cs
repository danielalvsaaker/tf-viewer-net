using HotChocolate;
using NetTopologySuite.Geometries;
using UnitsNet;

namespace Core;

public class Lap
{
    [GraphQLIgnore]
    public string ActivityId { get; private set; } = null!;
    [GraphQLIgnore]
    public string UserId { get; private set; } = null!;
    public required DateTimeOffset StartTime { get; init; }

    public required Point? StartPosition { get; init; }
    public required Point? EndPosition { get; init; }

    public required Duration Duration { get; init; }
    public required Duration DurationActive { get; init; }

    public required Length? Distance { get; init; }

    public required RotationalSpeed? CadenceAverage { get; init; }
    public required RotationalSpeed? CadenceMax { get; init; }

    public required Frequency? HeartRateAverage { get; init; }
    public required Frequency? HeartRateMax { get; init; }

    public required Speed? SpeedAverage { get; init; }
    public required Speed? SpeedMax { get; init; }

    public required Power? PowerAverage { get; init; }
    public required Power? PowerMax { get; init; }

    public required Length? Ascent { get; init; }
    public required Length? Descent { get; init; }

    public required Energy? Calories { get; init; }
}