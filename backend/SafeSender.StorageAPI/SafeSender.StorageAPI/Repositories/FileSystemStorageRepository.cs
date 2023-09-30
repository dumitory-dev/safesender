using Microsoft.Extensions.Options;
using SafeSender.StorageAPI.Interfaces;
using SafeSender.StorageAPI.Models;
using SafeSender.StorageAPI.Options;

namespace SafeSender.StorageAPI.Repositories;

/// <summary>
/// Local storage repository
/// </summary>
public class FileSystemStorageRepository : IFilesRepository
{
    private readonly IOptionsMonitor<StorageOptions> _storageOptions;
    private readonly ILogger<FileSystemStorageRepository> _logger;

    /// <summary>
    /// Constructor for <see cref="FileSystemStorageRepository"/>
    /// </summary>
    /// <param name="storageOptions">Storage options</param>
    /// <param name="logger">Logger</param>
    public FileSystemStorageRepository(IOptionsMonitor<StorageOptions> storageOptions, ILogger<FileSystemStorageRepository> logger)
    {
        _storageOptions = storageOptions;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<byte[]> GetFileBytes(string fileName)
    {
        var fullPath = GetFullPath(fileName);
        return await File.ReadAllBytesAsync(fullPath);
    }

    /// <inheritdoc />
    public async Task<FileSaveInfo> SaveFileBytes(string fileName, byte[] fileData)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("fileName cannot be null or empty.", nameof(fileName));
        }

        bool savingStatus;

        try
        {
            await File.WriteAllBytesAsync(GetFullPath(fileName), fileData);

            savingStatus = true;
        }
        catch (Exception e)
        {
            _logger.LogError(
                "{ClassName} - File saving failed. Message: {Message}. Stack trace: {StackTrace}. Inner exception: {InnerException}",
                nameof(FileSystemStorageRepository), e.Message, e.StackTrace, e.InnerException);

            savingStatus = false;
        }

        return new FileSaveInfo(savingStatus, fileName);
    }

    private string GetFullPath(string fileName)
    {
        return _storageOptions.CurrentValue.Filesystem.Path + Path.DirectorySeparatorChar + fileName;
    }
}