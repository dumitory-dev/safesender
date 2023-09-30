using System.Text;
using Flurl.Http;
using MessagePack;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using SafeSender.StorageAPI.Models.ApiModels;

namespace SafeSender.StorageAPI.Tests.IntegrationTests;

[TestFixture]
public class DownloadFileTests
{
    [Test]
    public async Task DownloadFile_CorrectTokenProvided_ReturnsFile()
    {
        // Arrange
        var client = SystemUnderTest.GetClient();
        var fileData = Encoding.UTF8.GetBytes("TestString");

        var uploadRequestModel = new UploadFileRequestModel
        {
            FileData = Encoding.UTF8.GetBytes("TestString"),
            FileName = "Test.txt",
            PasswordHash = Guid.NewGuid().ToString("N"),
            OriginalFileSize = 120000,
        };

        var fileBytes = MessagePackSerializer.Serialize(uploadRequestModel);
        
        // Act
        using var response = await client.Request(ApiConstants.UploadEndpointUrl).PostAsync(new ByteArrayContent(fileBytes));

        var responseModel = await response.GetJsonAsync<UploadFileResponseModel>();

        var downloadResponse = await client.Request(ApiConstants.DownloadEndpointUrl + $"/{responseModel.Token}").GetAsync();

        var downloadFileModel = MessagePackSerializer.Deserialize<DownloadFileResponseModel>(await downloadResponse.GetBytesAsync());

        // Assert
        Assert.AreEqual(fileData, downloadFileModel.FileData);
        Assert.AreEqual(uploadRequestModel.FileName, downloadFileModel.FileName);
        Assert.AreEqual(uploadRequestModel.OriginalFileSize, downloadFileModel.OriginalFileSize);
        Assert.AreEqual(uploadRequestModel.PasswordHash, downloadFileModel.PasswordHash);
        Assert.AreEqual(downloadResponse.StatusCode, StatusCodes.Status200OK);
    }
}