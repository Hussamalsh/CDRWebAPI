using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using CDRWebAPI.Models;

namespace CDRWebAPI.Services;

/// <summary>
/// This class implements the CSV file service interface and provides methods for parsing CSV files.
/// </summary>
public class CSVFileService : ICSVFileService
{
    /// <summary>
    /// Parses a CSV file asynchronously and returns a sequence of CDR records.
    /// </summary>
    /// <param name="file">The CSV file to parse.</param>
    /// <returns>An asynchronous enumerable of CDR records.</returns>
    public async IAsyncEnumerable<CDR?> ParseCSVFile(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
            MissingFieldFound = null
        };

        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<CDRMap>();

        // Process the file line by line
        while (await csv.ReadAsync())
        {
            var record = csv.GetRecord<CDR>();
            yield return record;
        }
    }
}

