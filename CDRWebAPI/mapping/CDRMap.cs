using CDRWebAPI.Converter;
using CDRWebAPI.Models;
using CsvHelper.Configuration;

public class CDRMap : ClassMap<CDR>
{
    public CDRMap()
    {
        Map(m => m.CallerId).Name("caller_id");
        Map(m => m.Recipient).Name("recipient");
        Map(m => m.CallDate).Name("call_date").TypeConverter<CustomDateTimeConverter>();
        Map(m => m.EndTime).Name("end_time").TypeConverter<CustomTimeSpanConverter>();
        Map(m => m.Duration).Name("duration");
        Map(m => m.Cost).Name("cost");
        Map(m => m.Reference).Name("reference");
        Map(m => m.Currency).Name("currency");
    }
}

