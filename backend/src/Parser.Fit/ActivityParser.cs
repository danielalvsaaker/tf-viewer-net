using Core;
using Dynastream.Fit;
using NetTopologySuite.Geometries;
using Parser.Fit.Extensions;
using UnitsNet.NumberExtensions.NumberToDuration;
using UnitsNet.NumberExtensions.NumberToSpeed;
using UnitsNet.NumberExtensions.NumberToEnergy;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToPower;
using UnitsNet.NumberExtensions.NumberToFrequency;
using UnitsNet.NumberExtensions.NumberToRotationalSpeed;
using Activity = Core.Activity;

namespace Parser.Fit;

public class ActivityParser
{
    private List<Session> Sessions { get; } = new();
    private List<Record> Records { get; } = new();
    private List<Lap> Laps { get; } = new();
    private Activity? Activity { get; set; }

    public Activity? Parse(Stream input)
    {
        var decoder = new Decode();
        var broadcaster = new MesgBroadcaster();

        broadcaster.RecordMesgEvent += OnRecordMessage;
        broadcaster.SessionMesgEvent += OnSessionMessage;
        broadcaster.LapMesgEvent += OnLapMessage;
        broadcaster.ActivityMesgEvent += OnActivityMessage;

        decoder.MesgEvent += broadcaster.OnMesg;

        return decoder.Read(input) switch
        {
            false => null,
            true => Activity,
        };
    }

    private void OnActivityMessage(object _, MesgEventArgs args)
    {
        if (args.mesg is not ActivityMesg activityMesg)
        {
            return;
        }

        Activity = new Activity
        {
            StartTime = Sessions.Select(session => session.StartTime).Order().First(),
            TotalTimerTime = activityMesg.GetTotalTimerTime()!.Value.Seconds(),
            BoundingBox = Sessions
                .Select(session => session.BoundingBox.EnvelopeInternal)
                .Aggregate((acc, envelope) => acc.ExpandedBy(envelope))
                .ToGeometry(),
            Sessions = Sessions,
            Records = Records,
            Laps = Laps
        };
    }

    private void OnSessionMessage(object _, MesgEventArgs args)
    {
        if (args.mesg is not SessionMesg sessionMesg)
        {
            return;
        }

        Sessions.Add(new Session
        {
            StartTime = sessionMesg.GetStartTime().GetDateTime(),

            BoundingBox = ((
                    sessionMesg.GetSwcLong(),
                    sessionMesg.GetSwcLat(),
                    sessionMesg.GetNecLong(),
                    sessionMesg.GetNecLat()) switch
            {
                ({ } swcLon, { } swcLat, { } necLon, { } necLat) => new Envelope(
                    new Coordinate(swcLon.SemicircleToDegree(), swcLat.SemicircleToDegree()),
                    new Coordinate(necLon.SemicircleToDegree(), necLat.SemicircleToDegree())),
                _ => new Envelope(),
            }).ToGeometry(),

            Duration = sessionMesg.GetTotalElapsedTime()!.Value.Seconds(),
            DurationActive = sessionMesg.GetTotalTimerTime()!.Value.Seconds(),

            Distance = sessionMesg.GetTotalDistance()?.Meters(),

            CadenceAverage = sessionMesg.GetAvgCadence()?.RevolutionsPerMinute(),
            CadenceMax = sessionMesg.GetMaxCadence()?.RevolutionsPerMinute(),

            HeartRateAverage = sessionMesg.GetAvgHeartRate()?.BeatsPerMinute(),
            HeartRateMax = sessionMesg.GetMaxHeartRate()?.BeatsPerMinute(),

            SpeedAverage = sessionMesg.GetEnhancedAvgSpeed()?.MetersPerSecond(),
            SpeedMax = sessionMesg.GetEnhancedMaxSpeed()?.MetersPerSecond(),

            PowerAverage = sessionMesg.GetAvgPower()?.Watts(),
            PowerMax = sessionMesg.GetMaxPower()?.Watts(),

            Ascent = sessionMesg.GetTotalAscent()?.Meters(),
            Descent = sessionMesg.GetTotalDescent()?.Meters(),

            Calories = sessionMesg.GetTotalCalories()?.Kilocalories(),
        });
    }

    private void OnRecordMessage(object _, MesgEventArgs args)
    {
        if (args.mesg is not RecordMesg recordMesg)
        {
            return;
        }

        Records.Add(new Record
        {
            Timestamp = recordMesg.GetTimestamp().GetDateTime(),
            Position = (recordMesg.GetPositionLong()?.SemicircleToDegree(), recordMesg.GetPositionLat()?.SemicircleToDegree()) switch
            {
                ({ } lon, { } lat) => new Point(lon, lat),
                _ => null,
            },
            Cadence = recordMesg.GetCadence()?.RevolutionsPerMinute(),
            Distance = recordMesg.GetDistance()?.Meters(),
            Altitude = recordMesg.GetEnhancedAltitude()?.Meters(),
            Speed = recordMesg.GetEnhancedSpeed()?.MetersPerSecond(),
            HeartRate = recordMesg.GetHeartRate()?.BeatsPerMinute(),
            Power = recordMesg.GetPower()?.Watts(),
        });
    }

    private void OnLapMessage(object _, MesgEventArgs args)
    {
        if (args.mesg is not LapMesg lapMesg)
        {
            return;
        }

        Laps.Add(new Lap
        {
            StartTime = lapMesg.GetStartTime().GetDateTime(),
            StartPosition = (lapMesg.GetStartPositionLong()?.SemicircleToDegree(), lapMesg.GetStartPositionLat()?.SemicircleToDegree()) switch
            {
                ({ } lon, { } lat) => new Point(lon, lat),
                _ => null,
            },
            EndPosition = (lapMesg.GetEndPositionLong()?.SemicircleToDegree(), lapMesg.GetEndPositionLat()?.SemicircleToDegree()) switch
            {
                ({ } lon, { } lat) => new Point(lon, lat),
                _ => null,
            },
            Duration = lapMesg.GetTotalElapsedTime()!.Value.Seconds(),
            DurationActive = lapMesg.GetTotalTimerTime()!.Value.Seconds(),
            Distance = lapMesg.GetTotalDistance()?.Meters(),
            CadenceAverage = lapMesg.GetAvgCadence()?.RevolutionsPerMinute(),
            CadenceMax = lapMesg.GetMaxCadence()?.RevolutionsPerMinute(),
            HeartRateAverage = lapMesg.GetAvgHeartRate()?.BeatsPerMinute(),
            HeartRateMax = lapMesg.GetMaxHeartRate()?.BeatsPerMinute(),
            SpeedAverage = lapMesg.GetEnhancedAvgSpeed()?.MetersPerSecond(),
            SpeedMax = lapMesg.GetEnhancedMaxSpeed()?.MetersPerSecond(),
            PowerAverage = lapMesg.GetAvgPower()?.Watts(),
            PowerMax = lapMesg.GetMaxPower()?.Watts(),
            Ascent = lapMesg.GetTotalAscent()?.Meters(),
            Descent = lapMesg.GetTotalDescent()?.Meters(),
            Calories = lapMesg.GetTotalCalories()?.Kilocalories(),
        });
    }
}