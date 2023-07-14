using CDRWebAPI.Models;

namespace CDRWebAPI.Services;

public interface ICSVFileService
{
    IAsyncEnumerable<CDR?> ParseCSVFile(IFormFile file);
}
