using System.Text;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using SafeSender.StorageAPI.Models;
using SafeSender.StorageAPI.Models.ApiModels;

namespace SafeSender.StorageAPI.Tests.IntegrationTests;

[TestFixture]
public class UploadFileTests
{
    private const string UploadEndpointUrl = "api/upload";
    
    [Test]
    public async Task UploadFile_FileUploaded_Returns201StatusCode()
    {
        // Arrange
        var client = SystemUnderTest.GetClient();
        var uploadedFileMock = Encoding.UTF8.GetBytes("mock test string 123");

        // Act
        using var response = await client.Request(UploadEndpointUrl).PostJsonAsync(new UploadFileRequestModel
        {
            FileBytes = uploadedFileMock,
            FileName = "text.txt",
            PasswordHash = Guid.NewGuid().ToString("N"),
        });

        var responseModel = await response.GetJsonAsync<UploadFileResponseModel>();
        
        // Assert
        Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        Assert.IsFalse(string.IsNullOrEmpty(responseModel.Token));
    }
}