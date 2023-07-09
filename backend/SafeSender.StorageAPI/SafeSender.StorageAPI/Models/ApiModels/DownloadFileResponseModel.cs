namespace SafeSender.StorageAPI.Models.ApiModels;

/// <summary>
/// Download file response model
/// </summary>
public class DownloadFileResponseModel
{
    /// <summary>
    /// Internal token
    /// </summary>
    public string ExternalStorageToken { get; set; } = default!;

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