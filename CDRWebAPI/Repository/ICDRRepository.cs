using CDRWebAPI.Models;

namespace CDRWebAPI.Repository;

public interface ICDRRepository
{
    Task AddCDRRangeAsync(IEnumerable<CDR> cdrs, CancellationToken cancellationToken = default);
    Task<decimal?> GetAverageCallCostAsync(CancellationToken cancellationToken = default);
    Task<List<CDR>> GetLongestCallsAsync(int top, CancellationToken cancellationToken = default);
    Task<double?> GetAverageNumberOfCallsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<decimal?> GetTotalCallCostAsync(CancellationToken cancellationToken = default);
    Task<int?> GetTotalNumberOfCallsAsync(CancellationToken cancellationToken = default);
    Task<string?> GetMostCalledNumberAsync(CancellationToken cancellationToken = default);
    Task<string?> GetMostActiveCallerAsync(CancellationToken cancellationToken = default);
    Task<decimal?> GetMinCallCostAsync(CancellationToken cancellationToken = default);
    Task<decimal?> GetMaxCallCostAsync(CancellationToken cancellationToken = default);
    Task<string?> GetFrequentCalledNumberAsync(CancellationToken cancellationToken = default);
    Task<int?> GetTotalCallDurationAsync(string callerId, CancellationToken cancellationToken = default);
}
