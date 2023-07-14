using Moq;
using Moq.EntityFrameworkCore;
using CDRWebAPI.DBContext;
using CDRWebAPI.Repository;
using CDRWebAPI.Models;

namespace CDRWebAPI.Tests
{
    public class CDRRepositoryTests
    {
        private Mock<ICDRContext> _contextMock;
        private List<CDR> _cdrList;
        private CDRRepository _cdrRepository;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<ICDRContext>();

            _cdrList = new List<CDR>()
            {
                new CDR { CallerId = "1", Recipient = "2", Duration = 30, Cost = 50m, Reference = "123", Currency = "USD", CallDate = DateTime.Now },
                new CDR { CallerId = "1", Recipient = "3", Duration = 60, Cost = 30m, Reference = "456", Currency = "USD", CallDate = DateTime.Now.AddDays(-1) },
            };

            _contextMock.Setup(x => x.CDRs).ReturnsDbSet(_cdrList);

            _cdrRepository = new CDRRepository(_contextMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _contextMock = null;
            _cdrList = null;
            _cdrRepository = null;
        }

        [Test]
        public async Task AddCDRRangeAsync_Should_Add_Valid_CDRs()
        {
            var cdrsToAdd = new List<CDR>()
            {
                new CDR { CallerId = "1", Recipient = "4", Duration = 30, Cost = 50m, Reference = "789", Currency = "USD", CallDate = DateTime.Now }
            };

            await _cdrRepository.AddCDRRangeAsync(cdrsToAdd);

            _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public void AddCDRRangeAsync_Should_Throw_Exception_When_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _cdrRepository.AddCDRRangeAsync(null));
        }

        [Test]
        public async Task GetAverageCallCostAsync_Should_Return_Correct_Average()
        {
            var expectedAverage = _cdrList.Average(x => x.Cost);

            var actualAverage = await _cdrRepository.GetAverageCallCostAsync();

            Assert.AreEqual(expectedAverage, actualAverage);
        }

        [Test]
        public async Task GetLongestCallsAsync_Should_Return_Correct_Top_Duration_Calls()
        {
            var top = 1;
            var expectedTopLongest = _cdrList.OrderByDescending(x => x.Duration).Take(top).ToList();

            var actualTopLongest = await _cdrRepository.GetLongestCallsAsync(top);

            CollectionAssert.AreEqual(expectedTopLongest, actualTopLongest);
        }

        [Test]
        public async Task GetAverageNumberOfCallsAsync_Should_Return_Correct_Average()
        {
            var startDate = DateTime.Now.AddDays(-1);
            var endDate = DateTime.Now;
            var expectedAverage = (double)_cdrList.Count(x => x.CallDate >= startDate && x.CallDate <= endDate) / (endDate - startDate).TotalDays;

            var actualAverage = await _cdrRepository.GetAverageNumberOfCallsAsync(startDate, endDate);

            Assert.AreEqual(expectedAverage, actualAverage);
        }

        [Test]
        public async Task GetTotalCallCostAsync_Should_Return_Correct_Total()
        {
            var expectedTotal = _cdrList.Sum(x => x.Cost);

            var actualTotal = await _cdrRepository.GetTotalCallCostAsync();

            Assert.AreEqual(expectedTotal, actualTotal);
        }

        [Test]
        public async Task GetTotalNumberOfCallsAsync_Should_Return_Correct_Count()
        {
            var expectedCount = _cdrList.Count;

            var actualCount = await _cdrRepository.GetTotalNumberOfCallsAsync();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetMostCalledNumberAsync_Should_Return_Correct_Number()
        {
            var expectedNumber = _cdrList.GroupBy(x => x.Recipient).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key;

            var actualNumber = await _cdrRepository.GetMostCalledNumberAsync();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [Test]
        public async Task GetMostActiveCallerAsync_Should_Return_Correct_CallerId()
        {
            var expectedCallerId = _cdrList.GroupBy(x => x.CallerId).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key;

            var actualCallerId = await _cdrRepository.GetMostActiveCallerAsync();

            Assert.AreEqual(expectedCallerId, actualCallerId);
        }

        [Test]
        public async Task GetMinCallCostAsync_Should_Return_Correct_Minimum()
        {
            var expectedMin = _cdrList.Min(x => x.Cost);

            var actualMin = await _cdrRepository.GetMinCallCostAsync();

            Assert.AreEqual(expectedMin, actualMin);
        }

        [Test]
        public async Task GetMaxCallCostAsync_Should_Return_Correct_Maximum()
        {
            var expectedMax = _cdrList.Max(x => x.Cost);

            var actualMax = await _cdrRepository.GetMaxCallCostAsync();

            Assert.AreEqual(expectedMax, actualMax);
        }

        [Test]
        public async Task GetTotalCallDurationAsync_Should_Return_Correct_Total()
        {
            var callerId = "1";
            var expectedTotal = _cdrList.Where(x => x.CallerId == callerId).Sum(x => x.Duration);

            var actualTotal = await _cdrRepository.GetTotalCallDurationAsync(callerId);

            Assert.AreEqual(expectedTotal, actualTotal);
        }

    }
}
