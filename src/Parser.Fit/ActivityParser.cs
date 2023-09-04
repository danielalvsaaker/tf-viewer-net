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
    private Session? _session = null;
    private readonly List<Record> _records = new List<Record>();
    private readonly List<Lap> _laps = new List<Lap>();

    public Activity Parse(Stream input)
    {
        var decoder = new Decode();
        var broadcaster = new MesgBroadcaster();

        broadcaster.RecordMesgEvent += OnRecordMessage;
        broadcaster.SessionMesgEvent += OnSessionMessage;
        broadcaster.LapMesgEvent += OnLapMessage;

        decoder.MesgEvent += broadcaster.OnMesg;

        var result = decoder.Read(input);

        return new Activity
        {
            Timestamp = _session.StartTime,
            Session = _session,
            Records = _records,
            Laps = _laps,
        };
    }

    private void OnSessionMessage(object _, MesgEventArgs args)
    {
        if (args.mesg is not SessionMesg sessionMesg)
        {
            return;
        }

        _session = new Session
        {
            StartTime = sessionMesg.GetStartTime().GetDateTime(),

            BoundingBox = (
                    sessionMesg.GetSwcLong(),
                    sessionMesg.GetSwcLat(),
                    sessionMesg.GetNecLong(),
                    sessionMesg.GetNecLat()) switch
                {
                    ({ } swcLon, { } swcLat, { } necLon, { } necLat) => new MultiPoint(new[]
                    {
                        new Point(swcLon.SemicircleToDegree(), swcLat.SemicircleToDegree()),
                        new Point(necLon.SemicircleToDegree(), necLat.SemicircleToDegree())
                    }),
                    _ => new MultiPoint(null)
                },

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
        };
    }

    private void OnRecordMessage(object _, MesgEventArgs args)
    {
        if (args.mesg is not RecordMesg recordMesg)
        {
            return;
        }

        _records.Add(new Record
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

        _laps.Add(new Lap
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