using CDRWebAPI.DBContext;
using CDRWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CDRWebAPI.Repository;

/// <summary>
/// This class implements the CDR repository interface and provides methods for CDR data operations.
/// </summary>
public class CDRRepository : ICDRRepository
{
    private readonly ICDRContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CDRRepository"/> class.
    /// </summary>
    /// <param name="context">The CDR context.</param>
    public CDRRepository(ICDRContext context)
    {
        _context = context;
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

        foreach (var cdr in cdrs)
        {
            if (cdr.CallerId == null || cdr.Recipient == null || cdr.Duration < 0 || cdr.Cost < 0 || cdr.Reference == null || cdr.Currency == null)
                throw new ArgumentException($"Invalid CDR record: {cdr}");
        }

        await _context.CDRs.AddRangeAsync(cdrs, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the average call cost asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the average call cost.</returns>
    public Task<decimal> GetAverageCallCostAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs.AverageAsync(c => c.Cost, cancellationToken);
        return query;
    }

    /// <summary>
    /// Retrieves the longest calls asynchronously.
    /// </summary>
    /// <param name="top">The number of longest calls to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the longest calls.</returns>
    public Task<List<CDR>> GetLongestCallsAsync(int top, CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs.OrderByDescending(c => c.Duration).Take(top).ToListAsync(cancellationToken);
        return query;
    }

    /// <summary>
    /// Retrieves the average number of calls within a specified date range asynchronously.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the average number of calls.</returns>
    public async Task<double?> GetAverageNumberOfCallsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var numberOfCalls = await _context.CDRs.CountAsync(c => c.CallDate >= startDate && c.CallDate <= endDate, cancellationToken);
        var totalDays = (endDate - startDate).TotalDays;

        // Avoid division by zero
        if (totalDays == 0)
        {
            return null;
        }

        return numberOfCalls / totalDays;
    }

    /// <summary>
    /// Retrieves the total call cost asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the total call cost.</returns>
    public Task<decimal> GetTotalCallCostAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs.SumAsync(c => c.Cost, cancellationToken);
        return query;
    }

    /// <summary>
    /// Retrieves the total number of calls asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the total number of calls.</returns>
    public Task<int> GetTotalNumberOfCallsAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs.CountAsync(cancellationToken);
        return query;
    }

    /// <summary>
    /// Retrieves the most called number asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the most called number.</returns>
    public Task<string> GetMostCalledNumberAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs
            .GroupBy(c => c.Recipient)
            .OrderByDescending(gp => gp.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync(cancellationToken);

        return query;
    }

    /// <summary>
    /// Retrieves the most active caller asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the most active caller.</returns>
    public Task<string?> GetMostActiveCallerAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs
            .GroupBy(c => c.CallerId)
            .OrderByDescending(gp => gp.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync(cancellationToken);

        return query;
    }

    /// <summary>
    /// Retrieves the minimum call cost asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the minimum call cost.</returns>
    public Task<decimal> GetMinCallCostAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs.MinAsync(c => c.Cost, cancellationToken);
        return query;
    }

    /// <summary>
    /// Retrieves the maximum call cost asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the maximum call cost.</returns>
    public Task<decimal> GetMaxCallCostAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs.MaxAsync(c => c.Cost, cancellationToken);
        return query;
    }

    /// <summary>
    /// Retrieves the most frequently called number asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the most frequently called number.</returns>
    public async Task<string> GetFrequentCalledNumberAsync(CancellationToken cancellationToken = default)
    {
        var query = await _context.CDRs
            .GroupBy(c => c.Recipient)
            .OrderByDescending(gp => gp.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync(cancellationToken);

        return query;
    }

    /// <summary>
    /// Retrieves the total call duration for a specific caller asynchronously.
    /// </summary>
    /// <param name="callerId">The caller ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and containing the total call duration.</returns>
    public Task<int> GetTotalCallDurationAsync(string callerId, CancellationToken cancellationToken = default)
    {
        var query = _context.CDRs
            .Where(c => c.CallerId == callerId)
            .SumAsync(c => c.Duration, cancellationToken);

        return query;
    }
}
