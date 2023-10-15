using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using SafeSender.StorageAPI.Database;
using SafeSender.StorageAPI.Models;

namespace SafeSender.StorageAPI.Jobs;

public class FileCleanupJob
{
    private readonly IMongoDbContext _context;
    private readonly ILogger<FileCleanupJob> _logger;

    public FileCleanupJob(IMongoDbContext context, ILogger<FileCleanupJob> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task CleanupExpiredFiles()
    {
        // TODO: Move days to config
        const int fileStoringTimeLimitInDays = 14;
     
        _logger.LogInformation("Files cleanup job execution started at UTC Time: {UtcTime}", DateTime.UtcNow.ToString("O"));
        
        var twoWeeksAgo = DateTime.UtcNow - TimeSpan.FromDays(fileStoringTimeLimitInDays);
        
        var entities = await _context.GridFSBucket.FindAsync(
            Builders<GridFSFileInfo>.Filter.Lte(f => f.UploadDateTime, twoWeeksAgo));

        var deletedCount = 0L;
        
        foreach (var id in entities.ToList().Select(f => f.Id))
        {
            await _context.GridFSBucket.DeleteAsync(id);

            var deleteResult = await _context.FilesInternalInfos.DeleteOneAsync(
                Builders<FileInternalInfo>.Filter.Eq(x => x.StorageFileIdentifier, id.ToString()));

            if (deleteResult.DeletedCount > 0)
            {
                _logger.LogInformation("File with StorageFileId {StorageFileId} was deleted", id);
                deletedCount += deleteResult.DeletedCount;
            }
        }
        
        _logger.LogInformation("Files cleanup job execution finished at UTC Time: {UtcTime}. Deleted count: {DeletedCount}", 
            DateTime.UtcNow.ToString("O"), 
            deletedCount);
    }
}