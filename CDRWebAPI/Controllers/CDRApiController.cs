using CDRWebAPI.Models;
using CDRWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
namespace CDRWebAPI.Controllers;

/// <summary>
/// This class represents the API controller for CDR (Call Detail Record) operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CDRApiController : ControllerBase
{
    private readonly ILogger<CDRApiController> _logger;
    private readonly ICDRService _cdrService;
    private readonly ICSVFileService _csvFileService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CDRApiController"/> class.
    /// </summary>
    /// <param name="cdrService">The CDR service.</param>
    /// <param name="csvFileService">The CSV file service.</param>
    /// <param name="logger">The logger.</param>
    public CDRApiController(ICDRService cdrService, ICSVFileService csvFileService, ILogger<CDRApiController> logger)
    {
        _cdrService = cdrService;
        _csvFileService = csvFileService;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint for uploading a CSV file containing CDR records.
    /// </summary>
    /// <param name="file">The uploaded CSV file.</param>
    /// <returns>The action result.</returns>
    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("File not selected for upload.");
            return BadRequest("File not selected");
        }

        try
        {
            var records = new List<CDR>();
            await foreach (var record in _csvFileService.ParseCSVFile(file))
            {
                if (record != null)
                {
                    records.Add(record);

                    // To prevent memory bloating, we insert records in chunks
                    if (records.Count >= 500)
                    {
                        await _cdrService.AddCDRRangeAsync(records);
                        records.Clear();
                    }
                }
            }

            // Insert any remaining records
            if (records.Any())
            {
                await _cdrService.AddCDRRangeAsync(records);
            }

            _logger.LogInformation("File uploaded successfully");
            return Ok(new { message = "File uploaded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while uploading file");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while uploading file");
        }
    }

    /// <summary>
    /// Retrieves the average call cost.
    /// </summary>
    /// <returns>The action result containing the average call cost.</returns>
    [HttpGet("AverageCallCost")]
    public async Task<IActionResult> GetAverageCallCost()
    {
        try
        {
            var averageCost = await _cdrService.GetAverageCallCostAsync();
            return Ok(new { averageCost });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching average call cost");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching average call cost");
        }
    }

    /// <summary>
    /// Retrieves the longest calls.
    /// </summary>
    /// <param name="top">The number of longest calls to retrieve.</param>
    /// <returns>The action result containing the longest calls.</returns>
    [HttpGet("LongestCalls")]
    public async Task<IActionResult> GetLongestCalls([FromQuery] int top = 10)
    {
        if (top < 1)
        {
            _logger.LogWarning("Invalid number of longest calls requested: {top}", top);
            return BadRequest("Invalid number of longest calls requested");
        }

        try
        {
            var longestCalls = await _cdrService.GetLongestCallsAsync(top);
            return Ok(longestCalls);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching longest calls");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching longest calls");
        }
    }

    /// <summary>
    /// Retrieves the average number of calls within a specified date range.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range.</param>
    /// <returns>The action result containing the average number of calls.</returns>
    [HttpGet("AverageNumberOfCalls")]
    public async Task<IActionResult> GetAverageNumberOfCalls([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            _logger.LogWarning("Invalid date range: {startDate} - {endDate}", startDate, endDate);
            return BadRequest("Invalid date range");
        }

        try
        {
            var averageCalls = await _cdrService.GetAverageNumberOfCallsAsync(startDate, endDate);
            return Ok(new { averageCalls });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching average number of calls");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching average number of calls");
        }
    }

    /// <summary>
    /// Retrieves the maximum call cost.
    /// </summary>
    /// <returns>The action result containing the maximum call cost.</returns>
    [HttpGet("MaxCallCost")]
    public async Task<IActionResult> GetMaxCallCost()
    {
        try
        {
            var maxCost = await _cdrService.GetMaxCallCostAsync();
            return Ok(new { maxCost });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching max call cost");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching max call cost");
        }
    }

    /// <summary>
    /// Retrieves the minimum call cost.
    /// </summary>
    /// <returns>The action result containing the minimum call cost.</returns>
    [HttpGet("MinCallCost")]
    public async Task<IActionResult> GetMinCallCost()
    {
        try
        {
            var minCost = await _cdrService.GetMinCallCostAsync();
            return Ok(new { minCost });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching min call cost");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching min call cost");
        }
    }

    /// <summary>
    /// Retrieves the most frequent called number.
    /// </summary>
    /// <returns>The action result containing the most frequent called number.</returns>
    [HttpGet("FrequentCalledNumber")]
    public async Task<IActionResult> GetFrequentCalledNumber()
    {
        try
        {
            var frequentNumber = await _cdrService.GetFrequentCalledNumberAsync();
            return Ok(new { frequentNumber });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching frequent called number");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching frequent called number");
        }
    }

    /// <summary>
    /// Retrieves the total call duration for a specific caller ID.
    /// </summary>
    /// <param name="callerId">The caller ID.</param>
    /// <returns>The action result containing the total call duration.</returns>
    [HttpGet("TotalCallDuration/{callerId}")]
    public async Task<IActionResult> GetTotalCallDuration(string callerId)
    {
        try
        {
            var totalDuration = await _cdrService.GetTotalCallDurationAsync(callerId);
            return Ok(new { totalDuration });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching total call duration for callerId {callerId}", callerId);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching total call duration");
        }
    }

    /// <summary>
    /// Retrieves the most called number.
    /// </summary>
    /// <returns>The action result containing the most called number.</returns>
    [HttpGet("MostCalledNumber")]
    public async Task<IActionResult> GetMostCalledNumber()
    {
        try
        {
            var mostCalledNumber = await _cdrService.GetMostCalledNumberAsync();
            return Ok(new { mostCalledNumber });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching the most called number");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching the most called number");
        }
    }

    /// <summary>
    /// Retrieves the most active caller.
    /// </summary>
    /// <returns>The action result containing the most active caller.</returns>
    [HttpGet("MostActiveCaller")]
    public async Task<IActionResult> GetMostActiveCaller()
    {
        try
        {
            var mostActiveCaller = await _cdrService.GetMostActiveCallerAsync();
            return Ok(new { mostActiveCaller });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching the most active caller");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching the most active caller");
        }
    }
}
