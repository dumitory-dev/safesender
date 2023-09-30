using System.Text;
using Flurl.Http;
using MessagePack;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using SafeSender.StorageAPI.Models.ApiModels;

namespace SafeSender.StorageAPI.Tests.IntegrationTests;

[TestFixture]
public class UploadFileTests
{
    [Test]
    public async Task UploadFile_FileUploaded_Returns200StatusCode()
    {
        // Arrange
        var client = SystemUnderTest.GetClient();

        var requestModel = new UploadFileRequestModel
        {
            FileData = Encoding.UTF8.GetBytes("TestString"),
            FileName = "Test.txt",
            PasswordHash = Guid.NewGuid().ToString("N"),
            OriginalFileSize = 120000,
        };

        var fileBytes = MessagePackSerializer.Serialize(requestModel);
        
        // Act
        using var response = await client.Request(ApiConstants.UploadEndpointUrl).PostAsync(new ByteArrayContent(fileBytes));
        
        var responseModel = await response.GetJsonAsync<UploadFileResponseModel>();

        // Assert
        Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        
        Assert.IsNotNull(responseModel.Token);
        Assert.IsNotEmpty(responseModel.Token);
    }
}