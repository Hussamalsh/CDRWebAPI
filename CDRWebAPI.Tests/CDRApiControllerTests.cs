using Moq;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using CDRWebAPI.Controllers;
using CDRWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CDRWebAPI.Models;

namespace CDRWebAPI.Tests;

[TestFixture]
public class CDRApiControllerTests
{
    private Mock<ICDRService> _cdrServiceMock;
    private Mock<ICSVFileService> _csvFileServiceMock;
    private Mock<ILogger<CDRApiController>> _loggerMock;
    private CDRApiController _controller;

    [SetUp]
    public void SetUp()
    {
        _cdrServiceMock = new Mock<ICDRService>();
        _csvFileServiceMock = new Mock<ICSVFileService>();
        _loggerMock = new Mock<ILogger<CDRApiController>>();
        _controller = new CDRApiController(_cdrServiceMock.Object, _csvFileServiceMock.Object, _loggerMock.Object);
    }

    #region Upload

    [Test]
    public async Task Upload_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var sourceImg = "CallerId,Recipient,CallDate,EndTime,Duration,Cost,Reference,Currency\n" +
                        "123456789,987654321,2023-07-13,2023-07-13,60,0.123,ABC123,USD";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(sourceImg));
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
        fileMock.Setup(f => f.Length).Returns(ms.Length);

        _csvFileServiceMock.Setup(s => s.ParseCSVFile(It.IsAny<IFormFile>())).Returns(GetTestRecords());

        // Act
        var result = await _controller.Upload(fileMock.Object);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    private async IAsyncEnumerable<CDR> GetTestRecords()
    {
        yield return new CDR { CallerId = "123456789", Recipient = "987654321", Reference = "ABC123" };
    }

    [Test]
    public async Task Upload_WhenFileIsNull_ReturnsBadRequest()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = await _controller.Upload(file);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Upload_WhenFileIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);

        // Act
        var result = await _controller.Upload(fileMock.Object);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Upload_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var ms = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
        fileMock.Setup(f => f.Length).Returns(ms.Length);

        _csvFileServiceMock.Setup(s => s.ParseCSVFile(It.IsAny<IFormFile>())).Throws<Exception>();

        // Act
        var result = await _controller.Upload(fileMock.Object);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion Upload

    #region GetAverageCallCost

    [Test]
    public async Task GetAverageCallCost_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var averageCost = new { averageCost = 0.123m };
        _cdrServiceMock.Setup(s => s.GetAverageCallCostAsync(It.IsAny<CancellationToken>())).ReturnsAsync(averageCost.averageCost);

        // Act
        var result = await _controller.GetAverageCallCost();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.AreEqual(averageCost.ToString(), okResult.Value.ToString());
    }

    [Test]
    public async Task GetAverageCallCost_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        _cdrServiceMock.Setup(s => s.GetAverageCallCostAsync(It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetAverageCallCost();

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetAverageCallCost

    #region GetLongestCalls

    [Test]
    public async Task GetLongestCalls_WhenCalledWithValidTop_ReturnsOkResult()
    {
        // Arrange
        var top = 5;
        var longestCalls = new List<CDR> { new CDR(), new CDR(), new CDR(), new CDR(), new CDR() };
        _cdrServiceMock.Setup(s => s.GetLongestCallsAsync(top, It.IsAny<CancellationToken>())).ReturnsAsync(longestCalls);

        // Act
        var result = await _controller.GetLongestCalls(top);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(longestCalls));
    }

    [Test]
    public async Task GetLongestCalls_WhenCalledWithInvalidTop_ReturnsBadRequest()
    {
        // Arrange
        var top = 0;

        // Act
        var result = await _controller.GetLongestCalls(top);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetLongestCalls_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        var top = 5;
        _cdrServiceMock.Setup(s => s.GetLongestCallsAsync(top, It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetLongestCalls(top);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetLongestCalls

    #region GetAverageNumberOfCalls

    [Test]
    public async Task GetAverageNumberOfCalls_WhenCalledWithValidDates_ReturnsOkResult()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var averageCalls = 10;
        _cdrServiceMock.Setup(s => s.GetAverageNumberOfCallsAsync(startDate, endDate, It.IsAny<CancellationToken>())).ReturnsAsync(averageCalls);

        // Act
        var result = await _controller.GetAverageNumberOfCalls(startDate, endDate);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value.ToString(), Is.EqualTo(new { averageCalls = averageCalls }.ToString()));
    }

    [Test]
    public async Task GetAverageNumberOfCalls_WhenCalledWithInvalidDates_ReturnsBadRequest()
    {
        // Arrange
        var startDate = new DateTime(2023, 12, 31);
        var endDate = new DateTime(2023, 1, 1);

        // Act
        var result = await _controller.GetAverageNumberOfCalls(startDate, endDate);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetAverageNumberOfCalls_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        _cdrServiceMock.Setup(s => s.GetAverageNumberOfCallsAsync(startDate, endDate, It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetAverageNumberOfCalls(startDate, endDate);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetAverageNumberOfCalls

    #region GetMaxCallCost

    [Test]
    public async Task GetMaxCallCost_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var maxCost = 10m;
        _cdrServiceMock.Setup(s => s.GetMaxCallCostAsync(It.IsAny<CancellationToken>())).ReturnsAsync(maxCost);

        // Act
        var result = await _controller.GetMaxCallCost();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value.ToString(), Is.EqualTo(new { maxCost = maxCost }.ToString()));
    }

    [Test]
    public async Task GetMaxCallCost_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        _cdrServiceMock.Setup(s => s.GetMaxCallCostAsync(It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetMaxCallCost();

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetMaxCallCost

    #region GetMinCallCost

    [Test]
    public async Task GetMinCallCost_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var minCost = 1m;
        _cdrServiceMock.Setup(s => s.GetMinCallCostAsync(It.IsAny<CancellationToken>())).ReturnsAsync(minCost);

        // Act
        var result = await _controller.GetMinCallCost();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value.ToString(), Is.EqualTo(new { minCost = minCost }.ToString()));
    }

    [Test]
    public async Task GetMinCallCost_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        _cdrServiceMock.Setup(s => s.GetMinCallCostAsync(It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetMinCallCost();

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetMinCallCost

    #region GetFrequentCalledNumber

    [Test]
    public async Task GetFrequentCalledNumber_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var frequentNumber = "123456789";
        _cdrServiceMock.Setup(s => s.GetFrequentCalledNumberAsync(It.IsAny<CancellationToken>())).ReturnsAsync(frequentNumber);

        // Act
        var result = await _controller.GetFrequentCalledNumber();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value.ToString(), Is.EqualTo(new { frequentNumber = frequentNumber }.ToString()));
    }

    [Test]
    public async Task GetFrequentCalledNumber_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        _cdrServiceMock.Setup(s => s.GetFrequentCalledNumberAsync(It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetFrequentCalledNumber();

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetFrequentCalledNumber

    #region GetTotalCallDuration

    [Test]
    public async Task GetTotalCallDuration_WhenCalledWithValidCallerId_ReturnsOkResult()
    {
        // Arrange
        var callerId = "123456789";
        var totalDuration = 3600;
        _cdrServiceMock.Setup(s => s.GetTotalCallDurationAsync(callerId, It.IsAny<CancellationToken>())).ReturnsAsync(totalDuration);

        // Act
        var result = await _controller.GetTotalCallDuration(callerId);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value.ToString(), Is.EqualTo(new { totalDuration = totalDuration }.ToString()));
    }

    [Test]
    public async Task GetTotalCallDuration_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        var callerId = "123456789";
        _cdrServiceMock.Setup(s => s.GetTotalCallDurationAsync(callerId, It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetTotalCallDuration(callerId);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetTotalCallDuration

    #region GetMostCalledNumber

    [Test]
    public async Task GetMostCalledNumber_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var mostCalledNumber = "987654321";
        _cdrServiceMock.Setup(s => s.GetMostCalledNumberAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mostCalledNumber);

        // Act
        var result = await _controller.GetMostCalledNumber();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value.ToString(), Is.EqualTo(new { mostCalledNumber = mostCalledNumber }.ToString()));
    }

    [Test]
    public async Task GetMostCalledNumber_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        _cdrServiceMock.Setup(s => s.GetMostCalledNumberAsync(It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetMostCalledNumber();

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetMostCalledNumber

    #region GetMostActiveCaller

    [Test]
    public async Task GetMostActiveCaller_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var mostActiveCaller = "123456789";
        _cdrServiceMock.Setup(s => s.GetMostActiveCallerAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mostActiveCaller);

        // Act
        var result = await _controller.GetMostActiveCaller();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value.ToString(), Is.EqualTo(new { mostActiveCaller = mostActiveCaller }.ToString()));
    }

    [Test]
    public async Task GetMostActiveCaller_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        // Arrange
        _cdrServiceMock.Setup(s => s.GetMostActiveCallerAsync(It.IsAny<CancellationToken>())).Throws<Exception>();

        // Act
        var result = await _controller.GetMostActiveCaller();

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion GetMostActiveCaller


}
