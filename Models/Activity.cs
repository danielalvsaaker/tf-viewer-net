using UnitsNet;

namespace Models;

public class Activity
{
    public int Id { get; set; }
    
    public required Session Session { get; init; }
    public required ICollection<Record> Records { get; init; }
    public required ICollection<Lap> Laps { get; init; }
}

public class Session
{
    public Length Distance { get; set; } 
    public Speed SpeedAverage { get; set; }
    public Speed SpeedMax { get; set; }
}

public class Record
{
   public Length Distance { get; set; }
   public Speed Speed { get; set; }
   public RotationalSpeed Cadence { get; set; }
   public Power Power { get; set; }
}

public class Lap
{
    public Length Distance { get; set; } 
    public Speed SpeedAverage { get; set; }
    public Speed SpeedMax { get; set; }
}