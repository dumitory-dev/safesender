using System.ComponentModel.DataAnnotations;

namespace SafeSender.StorageAPI.Models.ApiModels;

/// <summary>
/// Upload request file data request model
/// </summary>
public sealed class UploadFileRequestModel
{
    /// <summary>
    /// External storage file token
    /// </summary>
    [Required]
    public string ExternalStorageToken { get; set; } = default!;
    
    /// <summary>
    /// File name with extension (if extension provided)
    /// </summary>
    [Required]
    public string FileName { get; set; } = default!;
    
    /// <summary>
    /// Password hash
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = default!;
    
    /// <summary>
    /// File size before encryption
    /// </summary>
    [Required]
    public long OriginalFileSize { get; set; } = default!;
}