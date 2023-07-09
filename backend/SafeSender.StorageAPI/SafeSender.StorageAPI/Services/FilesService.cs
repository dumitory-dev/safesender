using Microsoft.Extensions.Options;
using SafeSender.StorageAPI.Interfaces;
using SafeSender.StorageAPI.Models;
using SafeSender.StorageAPI.Models.ApiModels;
using SafeSender.StorageAPI.Options;

namespace SafeSender.StorageAPI.Services;

/// <summary>
/// Files service
/// </summary>
public class FilesService : IFilesService
{
    private readonly IFilesInternalInfosRepository _filesInternalInfosRepository;
    private readonly IOptionsMonitor<StorageOptions> _storageOptions;
    private readonly ILogger<FilesService> _logger;

    /// <summary>
    /// Constructor for <see cref="FilesService"/>
    /// </summary>
    /// <param name="filesInternalInfosRepository">Files internal information repository</param>
    /// <param name="storageOptions">Storage options</param>
    /// <param name="logger">Logger</param>
    public FilesService(IFilesInternalInfosRepository filesInternalInfosRepository,
        IOptionsMonitor<StorageOptions> storageOptions, 
        ILogger<FilesService> logger)
    {
        _filesInternalInfosRepository = filesInternalInfosRepository;
        _storageOptions = storageOptions;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string> UploadFile(UploadFileRequestModel model)
    {
        var fileInternalInfo = new FileInternalInfo
        {
            FileName = model.FileName,
            StorageFileIdentifier = model.ExternalStorageToken,
            PasswordHash = model.PasswordHash,
            StorageType = _storageOptions.CurrentValue.Type,
            OriginalFileSize = model.OriginalFileSize,
        };
        
        await _filesInternalInfosRepository.Add(fileInternalInfo);

        _logger.LogInformation("UploadFile - File info added. External token: {ExternalToken}, Internal token: {InternalToken}", 
            fileInternalInfo.StorageFileIdentifier, fileInternalInfo.Token);
        
        return fileInternalInfo.Token;
    }

    /// <inheritdoc />
    public async Task<DownloadFileResponseModel> DownloadFile(string token)
    {
        var fileInternalInfo = await _filesInternalInfosRepository.GetByToken(token);
        
        if (fileInternalInfo is null)
        {
            throw new FileNotFoundException(
                $"DownloadFile - Information about file by specified token not found. Token: {token}");
        }

        return new()
        {
            ExternalStorageToken = fileInternalInfo.StorageFileIdentifier,
            FileName = fileInternalInfo.FileName,
            PasswordHash = fileInternalInfo.PasswordHash,
            OriginalFileSize = fileInternalInfo.OriginalFileSize,
        };
    }
}