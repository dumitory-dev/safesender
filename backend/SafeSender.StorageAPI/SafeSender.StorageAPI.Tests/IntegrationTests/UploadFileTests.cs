using Flurl.Http;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using SafeSender.StorageAPI.Models.ApiModels;

namespace SafeSender.StorageAPI.Tests.IntegrationTests;

[TestFixture]
public class UploadFileTests
{
    private const string UploadEndpointUrl = "api/upload";
    
    [Test]
    public async Task UploadFile_FileUploaded_Returns200StatusCode()
    {
        // Arrange
        var client = SystemUnderTest.GetClient();
        var externalStorageToken = "TestToken";

        // Act
        using var response = await client.Request(UploadEndpointUrl).PostJsonAsync(new UploadFileRequestModel
        {
            ExternalStorageToken = externalStorageToken,
            FileName = "Test.txt",
            PasswordHash = Guid.NewGuid().ToString("N"),
            OriginalFileSize = 120000,
        });

        // Assert
        Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
    }
}