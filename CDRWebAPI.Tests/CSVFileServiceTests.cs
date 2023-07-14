using CDRWebAPI.Models;
using CDRWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace CDRWebAPI.Tests;

[TestFixture]
public class CSVFileServiceTests
{
    private CSVFileService _csvFileService;

    [SetUp]
    public void SetUp()
    {
        _csvFileService = new CSVFileService();
    }

    [Test]
    public async Task ParseCSVFile_WhenCalledWithValidFile_ParsesFileCorrectly()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var sourceCSV = "caller_id,recipient,call_date,end_time,duration,cost,reference,currency\n" +
                        "441215598896,448000096481,16/08/2016,14:21:33,60,0.123,C5DA9724701EEBBA95CA2CC5617BA93E4,GBP";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(sourceCSV));
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        // Act
        var records = _csvFileService.ParseCSVFile(fileMock.Object);

        // Assert
        var recordList = new List<CDR>();
        await foreach (var record in records)
        {
            recordList.Add(record);
        }
        Assert.AreEqual(1, recordList.Count);
        Assert.AreEqual("441215598896", recordList[0].CallerId);
        Assert.AreEqual("448000096481", recordList[0].Recipient);
        Assert.AreEqual("C5DA9724701EEBBA95CA2CC5617BA93E4", recordList[0].Reference);
    }
}

