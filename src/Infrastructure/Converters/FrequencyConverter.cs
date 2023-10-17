using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitsNet;

namespace Infrastructure.Converters;

public class FrequencyConverter() : ValueConverter<Frequency, uint>(
    value => Convert.ToUInt32(value.BeatsPerMinute),
    value => Frequency.FromBeatsPerMinute(value));