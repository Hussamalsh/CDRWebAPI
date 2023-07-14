using CDRWebAPI.Models;
using CDRWebAPI.Repository;
using CDRWebAPI.Services;
using Moq;

namespace CDRWebAPI.Tests;

[TestFixture]
public class CDRServiceTests
{
    private Mock<ICDRRepository> _cdrRepositoryMock;
    private CDRService _cdrService;

    [SetUp]
    public void SetUp()
    {
        _cdrRepositoryMock = new Mock<ICDRRepository>();
        _cdrService = new CDRService(_cdrRepositoryMock.Object);
    }

    #region AddCDRRangeAsync

    [Test]
    public async Task AddCDRRangeAsync_WhenCalledWithValidCDRs_AddsCDRs()
    {
        // Arrange
        var cdrs = new List<CDR>
        {
            new CDR { CallerId = "123456789", Recipient = "987654321", Duration = 60, Cost = 0.123m, Reference = "ABC123", Currency = "USD" }
        };

        // Act
        await _cdrService.AddCDRRangeAsync(cdrs);

        // Assert
        _cdrRepositoryMock.Verify(r => r.AddCDRRangeAsync(cdrs, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void AddCDRRangeAsync_WhenCalledWithNullCDRs_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<CDR> cdrs = null;

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _cdrService.AddCDRRangeAsync(cdrs));
    }

    #endregion AddCDRRangeAsync

    #region GetAverageCallCostAsync

    [Test]
    public async Task GetAverageCallCostAsync_WhenCalled_ReturnsAverageCallCost()
    {
        // Arrange
        var averageCallCost = 0.123m;
        _cdrRepositoryMock.Setup(r => r.GetAverageCallCostAsync(It.IsAny<CancellationToken>())).ReturnsAsync(averageCallCost);

        // Act
        var result = await _cdrService.GetAverageCallCostAsync();

        // Assert
        Assert.AreEqual(averageCallCost, result);
        _cdrRepositoryMock.Verify(r => r.GetAverageCallCostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetAverageCallCostAsync

    #region GetLongestCallsAsync

    [Test]
    public async Task GetLongestCallsAsync_WhenCalled_ReturnsLongestCalls()
    {
        // Arrange
        var top = 5;
        var longestCalls = new List<CDR> { new CDR(), new CDR(), new CDR(), new CDR(), new CDR() };
        _cdrRepositoryMock.Setup(r => r.GetLongestCallsAsync(top, It.IsAny<CancellationToken>())).ReturnsAsync(longestCalls);

        // Act
        var result = await _cdrService.GetLongestCallsAsync(top);

        // Assert
        Assert.AreEqual(longestCalls, result);
        _cdrRepositoryMock.Verify(r => r.GetLongestCallsAsync(top, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetLongestCallsAsync

    #region GetAverageNumberOfCallsAsync

    [Test]
    public async Task GetAverageNumberOfCallsAsync_WhenCalled_ReturnsAverageNumberOfCalls()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var averageNumberOfCalls = 10.0;
        _cdrRepositoryMock.Setup(r => r.GetAverageNumberOfCallsAsync(startDate, endDate, It.IsAny<CancellationToken>())).ReturnsAsync(averageNumberOfCalls);

        // Act
        var result = await _cdrService.GetAverageNumberOfCallsAsync(startDate, endDate);

        // Assert
        Assert.AreEqual(averageNumberOfCalls, result);
        _cdrRepositoryMock.Verify(r => r.GetAverageNumberOfCallsAsync(startDate, endDate, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetAverageNumberOfCallsAsync

    #region GetTotalCallCostAsync

    [Test]
    public async Task GetTotalCallCostAsync_WhenCalled_ReturnsTotalCallCost()
    {
        // Arrange
        var totalCallCost = 123.45m;
        _cdrRepositoryMock.Setup(r => r.GetTotalCallCostAsync(It.IsAny<CancellationToken>())).ReturnsAsync(totalCallCost);

        // Act
        var result = await _cdrService.GetTotalCallCostAsync();

        // Assert
        Assert.AreEqual(totalCallCost, result);
        _cdrRepositoryMock.Verify(r => r.GetTotalCallCostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetTotalCallCostAsync

    #region GetTotalNumberOfCallsAsync

    [Test]
    public async Task GetTotalNumberOfCallsAsync_WhenCalled_ReturnsTotalNumberOfCalls()
    {
        // Arrange
        var totalNumberOfCalls = 100;
        _cdrRepositoryMock.Setup(r => r.GetTotalNumberOfCallsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(totalNumberOfCalls);

        // Act
        var result = await _cdrService.GetTotalNumberOfCallsAsync();

        // Assert
        Assert.AreEqual(totalNumberOfCalls, result);
        _cdrRepositoryMock.Verify(r => r.GetTotalNumberOfCallsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetTotalNumberOfCallsAsync

    #region GetMostCalledNumberAsync

    [Test]
    public async Task GetMostCalledNumberAsync_WhenCalled_ReturnsMostCalledNumber()
    {
        // Arrange
        var mostCalledNumber = "123456789";
        _cdrRepositoryMock.Setup(r => r.GetMostCalledNumberAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mostCalledNumber);

        // Act
        var result = await _cdrService.GetMostCalledNumberAsync();

        // Assert
        Assert.AreEqual(mostCalledNumber, result);
        _cdrRepositoryMock.Verify(r => r.GetMostCalledNumberAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetMostCalledNumberAsync

    #region GetMostActiveCallerAsync

    [Test]
    public async Task GetMostActiveCallerAsync_WhenCalled_ReturnsMostActiveCaller()
    {
        // Arrange
        var mostActiveCaller = "123456789";
        _cdrRepositoryMock.Setup(r => r.GetMostActiveCallerAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mostActiveCaller);

        // Act
        var result = await _cdrService.GetMostActiveCallerAsync();

        // Assert
        Assert.AreEqual(mostActiveCaller, result);
        _cdrRepositoryMock.Verify(r => r.GetMostActiveCallerAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetMostActiveCallerAsync

    #region GetMinCallCostAsync

    [Test]
    public async Task GetMinCallCostAsync_WhenCalled_ReturnsMinCallCost()
    {
        // Arrange
        var minCallCost = 0.01m;
        _cdrRepositoryMock.Setup(r => r.GetMinCallCostAsync(It.IsAny<CancellationToken>())).ReturnsAsync(minCallCost);

        // Act
        var result = await _cdrService.GetMinCallCostAsync();

        // Assert
        Assert.AreEqual(minCallCost, result);
        _cdrRepositoryMock.Verify(r => r.GetMinCallCostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetMinCallCostAsync

    #region GetMaxCallCostAsync

    [Test]
    public async Task GetMaxCallCostAsync_WhenCalled_ReturnsMaxCallCost()
    {
        // Arrange
        var maxCallCost = 123.45m;
        _cdrRepositoryMock.Setup(r => r.GetMaxCallCostAsync(It.IsAny<CancellationToken>())).ReturnsAsync(maxCallCost);

        // Act
        var result = await _cdrService.GetMaxCallCostAsync();

        // Assert
        Assert.AreEqual(maxCallCost, result);
        _cdrRepositoryMock.Verify(r => r.GetMaxCallCostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetMaxCallCostAsync

    #region GetFrequentCalledNumberAsync

    [Test]
    public async Task GetFrequentCalledNumberAsync_WhenCalled_ReturnsFrequentCalledNumber()
    {
        // Arrange
        var frequentCalledNumber = "123456789";
        _cdrRepositoryMock.Setup(r => r.GetFrequentCalledNumberAsync(It.IsAny<CancellationToken>())).ReturnsAsync(frequentCalledNumber);

        // Act
        var result = await _cdrService.GetFrequentCalledNumberAsync();

        // Assert
        Assert.AreEqual(frequentCalledNumber, result);
        _cdrRepositoryMock.Verify(r => r.GetFrequentCalledNumberAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetFrequentCalledNumberAsync

    #region GetTotalCallDurationAsync

    [Test]
    public async Task GetTotalCallDurationAsync_WhenCalled_ReturnsTotalCallDuration()
    {
        // Arrange
        var callerId = "123456789";
        var totalCallDuration = 3600;
        _cdrRepositoryMock.Setup(r => r.GetTotalCallDurationAsync(callerId, It.IsAny<CancellationToken>())).ReturnsAsync(totalCallDuration);

        // Act
        var result = await _cdrService.GetTotalCallDurationAsync(callerId);

        // Assert
        Assert.AreEqual(totalCallDuration, result);
        _cdrRepositoryMock.Verify(r => r.GetTotalCallDurationAsync(callerId, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetTotalCallDurationAsync

}
