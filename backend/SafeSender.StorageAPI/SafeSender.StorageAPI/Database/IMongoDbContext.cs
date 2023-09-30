using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using SafeSender.StorageAPI.Models;

namespace SafeSender.StorageAPI.Database;

public interface IMongoDbContext
{
    public IMongoCollection<FileInternalInfo> FilesInternalInfos { get; init; }
    public IGridFSBucket GridFSBucket { get; init; }
}