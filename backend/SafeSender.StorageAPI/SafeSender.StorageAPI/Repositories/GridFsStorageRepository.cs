using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using SafeSender.StorageAPI.Database;
using SafeSender.StorageAPI.Interfaces;
using SafeSender.StorageAPI.Models;

namespace SafeSender.StorageAPI.Repositories;

public class GridFsStorageRepository : IFilesRepository
{
    private readonly IMongoDbContext _context;

    public GridFsStorageRepository(IMongoDbContext context)
    {
        _context = context;
    }
    
    public async Task<byte[]> GetFileBytes(string fileHash)
    {
        var isValidId = ObjectId.TryParse(fileHash, out var id);

        if (!isValidId)
        {
            throw new ValidationException("Invalid internal storage id");
        }
        
        return await _context.GridFSBucket.DownloadAsBytesAsync(id);
    }

    public async Task<FileSaveInfo> SaveFileBytes(string fileName, byte[] fileData)
    {
        var id = await _context.GridFSBucket.UploadFromBytesAsync(fileName, fileData);
        
        return new FileSaveInfo(id != ObjectId.Empty, id.ToString() ?? string.Empty);
    }
}