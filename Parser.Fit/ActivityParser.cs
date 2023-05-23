using System.Runtime.InteropServices.ComTypes;
using Core;
using Dynastream.Fit;
using Microsoft.VisualBasic;
using NetTopologySuite.DataStructures;
using NetTopologySuite.Geometries;
using UnitsNet;
using Activity = Core.Activity;

namespace Parser.Fit;

static class NullableExtension
{
    public static TResult? Map<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector) 
        where TSource : struct
        where TResult : struct
    {
        return source.HasValue ? selector(source.Value) : new TResult?();
    }
    
    public static TResult? Map<TSource1, TSource2, TResult>(this (TSource1? first, TSource2? second) source, Func<(TSource1, TSource2), TResult> selector) 
        where TSource1 : struct
        where TSource2 : struct
        where TResult : class
    {
        return source is { first: not null, second: not null } ? selector((source.first.Value, source.second.Value)) : null;
    }
}

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
        Console.WriteLine(decoder.IsFIT(input));
        Console.WriteLine(result);

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

            NorthEastCorner = (sessionMesg.GetNecLong(), sessionMesg.GetNecLat())
                .Map(corner =>
                {
                    var (lon, lat) = corner;
                    return new Point(lon, lat);
                }),
            SouthWestCorner = (sessionMesg.GetSwcLong(), sessionMesg.GetSwcLat())
                .Map(corner =>
                {
                    var (lon, lat) = corner;
                    return new Point(lon, lat);
                }),

            Duration = sessionMesg.GetTotalElapsedTime()
                .Map(duration => Duration.FromSeconds(duration))!
                .Value,
            DurationActive = sessionMesg.GetTotalTimerTime()
                .Map(duration => Duration.FromSeconds(duration))!
                .Value,

            Distance = sessionMesg.GetTotalDistance()
                .Map(distance => Length.FromMeters(distance)),

            CadenceAverage = sessionMesg.GetAvgCadence()
                .Map(cadence => RotationalSpeed.FromRevolutionsPerMinute(cadence)),
            CadenceMax = sessionMesg.GetMaxCadence()
                .Map(cadence => RotationalSpeed.FromRevolutionsPerMinute(cadence)),

            HeartRateAverage = sessionMesg.GetAvgHeartRate()
                .Map(heartRate => Frequency.FromBeatsPerMinute(heartRate)),
            HeartRateMax = sessionMesg.GetMaxHeartRate()
                .Map(heartRate => Frequency.FromBeatsPerMinute(heartRate)),

            SpeedAverage = sessionMesg.GetEnhancedAvgSpeed()
                .Map(speed => Speed.FromMetersPerSecond(speed)),
            SpeedMax = sessionMesg.GetEnhancedMaxSpeed()
                .Map(speed => Speed.FromMetersPerSecond(speed)),

            PowerAverage = sessionMesg.GetAvgPower()
                .Map(power => Power.FromWatts(power)),
            PowerMax = sessionMesg.GetMaxPower()
                .Map(power => Power.FromWatts(power)),
            
            Ascent = sessionMesg.GetTotalAscent()
                .Map(ascent => Length.FromMeters(ascent)),
            Descent = sessionMesg.GetTotalDescent()
                .Map(descent => Length.FromMeters(descent)),
            
            Calories = sessionMesg.GetTotalCalories()
                .Map(calories => Energy.FromKilocalories(calories)),
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
            Position = (recordMesg.GetPositionLong(), recordMesg.GetPositionLat())
                .Map(position =>
                {
                    var (lon, lat) = position;
                    return new Point(lon, lat);
                }),
            Cadence = recordMesg.GetCadence()
                .Map(cadence => RotationalSpeed.FromRevolutionsPerMinute(cadence)),
            Distance = recordMesg.GetDistance()
                .Map(distance => Length.FromMeters(distance)),
            Altitude = recordMesg.GetEnhancedAltitude()
                .Map(altitude => Length.FromMeters(altitude)),
            Speed = recordMesg.GetEnhancedSpeed()
                .Map(speed => Speed.FromMetersPerSecond(speed)),
            HeartRate = recordMesg.GetHeartRate()
                .Map(heartRate => Frequency.FromBeatsPerMinute(heartRate)),
            Power = recordMesg.GetPower()
                .Map(power => Power.FromWatts(power)),
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
            StartPosition = (lapMesg.GetStartPositionLong(), lapMesg.GetStartPositionLat())
                .Map(position =>
                {
                    var (lon, lat) = position;
                    return new Point(lon, lat);
                }),
            EndPosition = (lapMesg.GetEndPositionLong(), lapMesg.GetEndPositionLat())
                .Map(position =>
                {
                    var (lon, lat) = position;
                    return new Point(lon, lat);
                }),
            Duration = lapMesg.GetTotalElapsedTime()
                .Map(duration => Duration.FromSeconds(duration))!
                .Value,
            DurationActive = lapMesg.GetTotalTimerTime()
                .Map(duration => Duration.FromSeconds(duration))!
                .Value,
            Distance = lapMesg.GetTotalDistance()
                .Map(distance => Length.FromMeters(distance)),
            CadenceAverage = lapMesg.GetAvgCadence()
                .Map(cadence => RotationalSpeed.FromRevolutionsPerMinute(cadence)),
            CadenceMax = lapMesg.GetMaxCadence()
                .Map(cadence => RotationalSpeed.FromRevolutionsPerMinute(cadence)),
            HeartRateAverage = lapMesg.GetAvgHeartRate()
                .Map(heartRate => Frequency.FromBeatsPerMinute(heartRate)),
            HeartRateMax = lapMesg.GetMaxHeartRate()
                .Map(heartRate => Frequency.FromBeatsPerMinute(heartRate)),
            SpeedAverage = lapMesg.GetEnhancedAvgSpeed()
                .Map(speed => Speed.FromMetersPerSecond(speed)),
            SpeedMax = lapMesg.GetEnhancedMaxSpeed()
                .Map(speed => Speed.FromMetersPerSecond(speed)),
            PowerAverage = lapMesg.GetAvgPower()
                .Map(power => Power.FromWatts(power)),
            PowerMax = lapMesg.GetMaxPower()
                .Map(power => Power.FromWatts(power)),
            Ascent = lapMesg.GetTotalAscent()
                .Map(ascent => Length.FromMeters(ascent)),
            Descent = lapMesg.GetTotalDescent()
                .Map(descent => Length.FromMeters(descent)),
            Calories = lapMesg.GetTotalCalories()
                .Map(calories => Energy.FromKilocalories(calories))
        });
    }
}