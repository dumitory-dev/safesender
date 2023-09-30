using System.ComponentModel.DataAnnotations;
using MessagePack;

namespace SafeSender.StorageAPI.Models.ApiModels;

/// <summary>
/// Upload request file data request model
/// </summary>
[MessagePackObject(keyAsPropertyName: true)]
public sealed class UploadFileRequestModel
{
    /// <summary>
    /// File data byte array
    /// </summary>
    [Required]
    public byte[] FileData { get; set; } = Array.Empty<byte>();
    
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
    public long OriginalFileSize { get; set; }
}