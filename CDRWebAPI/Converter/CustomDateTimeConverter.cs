using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using CsvHelper.TypeConversion;

namespace CDRWebAPI.Converter;

/// <summary>
/// This class extends the DateTimeConverter class and provides custom date and time conversion logic for CSV parsing.
/// </summary>
public class CustomDateTimeConverter : DateTimeConverter
{
    /// <summary>
    /// Converts a string representation of a date or date and time to a DateTime object.
    /// </summary>
    /// <param name="text">The string representation of the date or date and time.</param>
    /// <param name="row">The current reader row.</param>
    /// <param name="memberMapData">The member map data.</param>
    /// <returns>The converted DateTime object.</returns>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        var formatProvider = (IFormatProvider)row.Configuration.CultureInfo.DateTimeFormat.Clone();
        var dateTimeFormat = (DateTimeFormatInfo)formatProvider.GetFormat(typeof(DateTimeFormatInfo));
        dateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

        if (DateTime.TryParseExact(text, "dd/MM/yyyy HH:mm:ss", formatProvider, DateTimeStyles.None, out var datetime))
            return datetime;
        else if (DateTime.TryParseExact(text, "dd/MM/yyyy", formatProvider, DateTimeStyles.None, out datetime))
            return datetime;
        else
            throw new FormatException($"'{text}' is not a valid date or date and time");
    }
}


