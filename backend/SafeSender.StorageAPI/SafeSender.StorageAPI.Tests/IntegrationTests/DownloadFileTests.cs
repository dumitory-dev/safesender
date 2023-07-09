using Flurl.Http;
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
        var externalStorageToken = "TestToken";
        
        // Act
        using var response = await client.Request(ApiConstants.UploadEndpointUrl).PostJsonAsync(new UploadFileRequestModel
        {
            ExternalStorageToken = externalStorageToken,
            FileName = "text.txt",
            PasswordHash = Guid.NewGuid().ToString("N"),
            OriginalFileSize = 120000,
        });

        var responseModel = await response.GetJsonAsync<UploadFileResponseModel>();

        var downloadResponse = await client.Request(ApiConstants.DownloadEndpointUrl + $"/{responseModel.Token}").GetAsync();
        var downloadFileModel = await downloadResponse.GetJsonAsync<DownloadFileResponseModel>();

        // Assert
        Assert.AreEqual(externalStorageToken, downloadFileModel.ExternalStorageToken);
        Assert.AreEqual(downloadResponse.StatusCode, StatusCodes.Status200OK);
    }
}