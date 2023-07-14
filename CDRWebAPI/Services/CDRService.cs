using CDRWebAPI.Models;
using CDRWebAPI.Repository;

namespace CDRWebAPI.Services;

/// <summary>
/// This class implements the CDR service interface and provides methods for CDR operations.
/// </summary>
public class CDRService : ICDRService
{
    private readonly ICDRRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CDRService"/> class.
    /// </summary>
    /// <param name="repository">The CDR repository.</param>
    public CDRService(ICDRRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Adds a range of CDR records asynchronously.
    /// </summary>
    /// <param name="cdrs">The collection of CDR records to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddCDRRangeAsync(IEnumerable<CDR> cdrs, CancellationToken cancellationToken = default)
    {
        if (cdrs == null) throw new ArgumentNullException(nameof(cdrs));
        if (!cdrs.Any()) return;

        await _repository.AddCDRRangeAsync(cdrs, cancellationToken);
    }

    /// <summary>
    /// Retrieves the average call cost asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the average call cost.</returns>
    public Task<decimal?> GetAverageCallCostAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAverageCallCostAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the longest calls asynchronously.
    /// </summary>
    /// <param name="top">The number of longest calls to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the longest calls.</returns>
    public Task<List<CDR>> GetLongestCallsAsync(int top, CancellationToken cancellationToken = default)
    {
        return _repository.GetLongestCallsAsync(top, cancellationToken);
    }

    /// <summary>
    /// Retrieves the average number of calls within a specified date range asynchronously.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the average number of calls.</returns>
    public Task<double?> GetAverageNumberOfCallsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return _repository.GetAverageNumberOfCallsAsync(startDate, endDate, cancellationToken);
    }

    /// <summary>
    /// Retrieves the total call cost asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the total call cost.</returns>
    public Task<decimal?> GetTotalCallCostAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetTotalCallCostAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the total number of calls asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the total number of calls.</returns>
    public Task<int?> GetTotalNumberOfCallsAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetTotalNumberOfCallsAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the most called number asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the most called number.</returns>
    public Task<string?> GetMostCalledNumberAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetMostCalledNumberAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the most active caller asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the most active caller.</returns>
    public Task<string?> GetMostActiveCallerAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetMostActiveCallerAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the minimum call cost asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the minimum call cost.</returns>
    public Task<decimal?> GetMinCallCostAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetMinCallCostAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the maximum call cost asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the maximum call cost.</returns>
    public Task<decimal?> GetMaxCallCostAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetMaxCallCostAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the most frequent called number asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the most frequent called number.</returns>
    public Task<string?> GetFrequentCalledNumberAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetFrequentCalledNumberAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the total call duration for a specific caller ID asynchronously.
    /// </summary>
    /// <param name="callerId">The caller ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the total call duration.</returns>
    public Task<int?> GetTotalCallDurationAsync(string callerId, CancellationToken cancellationToken = default)
    {
        return _repository.GetTotalCallDurationAsync(callerId, cancellationToken);
    }
}


