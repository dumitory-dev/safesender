namespace SafeSender.StorageAPI.Models.Enums;

/// <summary>
/// Storage type
/// </summary>
public enum StorageType
{
    /// <summary>
    /// External storage
    /// </summary>
    External,
    
    /// <summary>
    /// Local filesystem storage
    /// </summary>
    Filesystem,
    
    /// <summary>
    /// MongoDB GridFS storage
    /// </summary>
    GridFS,
}