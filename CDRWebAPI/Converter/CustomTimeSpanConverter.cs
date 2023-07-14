using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace CDRWebAPI.Converter;

/// <summary>
/// This class extends the TimeSpanConverter class and provides custom time span conversion logic for CSV parsing.
/// </summary>
public class CustomTimeSpanConverter : TimeSpanConverter
{
    /// <summary>
    /// Converts a string representation of a time span to a TimeSpan object.
    /// </summary>
    /// <param name="text">The string representation of the time span.</param>
    /// <param name="row">The current reader row.</param>
    /// <param name="memberMapData">The member map data.</param>
    /// <returns>The converted TimeSpan object.</returns>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (TimeSpan.TryParseExact(text, "hh\\:mm\\:ss", null, out var time))
            return time;
        else
            throw new FormatException($"'{text}' is not a valid time");
    }
}

