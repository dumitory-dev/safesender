using MessagePack;

namespace SafeSender.StorageAPI.Models.ApiModels;

/// <summary>
/// Download file response model
/// </summary>
[MessagePackObject(keyAsPropertyName: true)]
public class DownloadFileResponseModel
{
    /// <summary>
    /// File data byte array
    /// </summary>
    public byte[] FileData { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// File name with extension
    /// </summary>
    public string FileName { get; set; } = default!;
    
    /// <summary>
    /// Password for file hash
    /// </summary>
    public string PasswordHash { get; set; } = default!;
    
    /// <summary>
    /// File size before encryption
    /// </summary>
    public long OriginalFileSize { get; set; }
}