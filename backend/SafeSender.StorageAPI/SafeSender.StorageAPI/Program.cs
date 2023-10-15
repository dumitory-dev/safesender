using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AnonFilesApi.Implementations;
using AnonFilesApi.Interfaces;
using Hangfire;
using MessagePack;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using SafeSender.StorageAPI.Database;
using SafeSender.StorageAPI.Extensions;
using SafeSender.StorageAPI.Interfaces;
using SafeSender.StorageAPI.Jobs;
using SafeSender.StorageAPI.Middlewares;
using SafeSender.StorageAPI.Models.ApiModels;
using SafeSender.StorageAPI.Models.Enums;
using SafeSender.StorageAPI.Options;
using SafeSender.StorageAPI.Repositories;
using SafeSender.StorageAPI.Services;

[assembly: InternalsVisibleTo("SafeSender.StorageAPI.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Services.AddHttpLogging(
    logger =>
    {
        logger.LoggingFields = HttpLoggingFields.Request
                               | HttpLoggingFields.ResponseStatusCode
                               | HttpLoggingFields.ResponseHeaders
                               | HttpLoggingFields.ResponseBody;

        logger.ResponseHeaders.Add("Location");
        logger.RequestHeaders.Add("Origin");
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddSwaggerGen(options =>
// {
//     var comments = Path.Combine(builder.Environment.ContentRootPath, "SafeSender.StorageAPI.xml");
//     options.IncludeXmlComments(comments);
// });


// Add Cors
const string corsPolicyAcceptAllName = "AcceptAll";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyAcceptAllName,
        policyBuilder => policyBuilder
            .SetIsOriginAllowed(host => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var storageSettingsSection = builder.Configuration.GetSection(StorageOptions.SettingsPath);

// Options registration
builder.Services.Configure<StorageOptions>(storageSettingsSection);
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("Application:Database"));

// Repositories registration
var repositoryType = GetFileStorageRepositoryType(storageSettingsSection);
builder.Services.AddScoped(typeof(IFilesRepository), repositoryType);

builder.Services.AddScoped<IFilesInternalInfosRepository, FileInternalInfosRepository>();

// DB registration
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();

// API clients registration
builder.Services.AddScoped<IAnonfilesApiClient, AnonfilesApiClient>();

// Services registration
builder.Services.AddScoped<IFilesService, FilesService>();

//Jobs registration
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();

if (!builder.Environment.IsDevelopment())
{
    app.UseHttpLogging();
}

// TODO: Add authorization to use Hangfire dashboard in production
if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new FreeDashboardAuthorization() }
    });
    
    app.MapHangfireDashboard();
}

RecurringJob.AddOrUpdate<FileCleanupJob>("CleanupExpiredFilesJob", job => job.CleanupExpiredFiles(), Cron.Daily);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(corsPolicyAcceptAllName);

// TODO: Replace with .AddHealthChecks() / .MapHealthChecks()
app.MapGet("api/health", () => Results.Ok("Healthy as hell"));

app.MapGet("api/download/{token}", async (
        [Required] string token,
        [FromServices] IFilesService filesService) =>
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Results.BadRequest("Token cannot be null, empty or white space");
        }

        var downloadFileModel = await filesService.DownloadFile(token);

        return Results.Bytes(MessagePackSerializer.Serialize(downloadFileModel), "application/octet-stream");
    })
    .WithName("GetFile")
    .Produces<DownloadFileResponseModel>()
    .Produces(StatusCodes.Status400BadRequest);

app.MapPost("api/upload", async (
        HttpRequest request,
        [FromServices] IFilesService filesService) =>
    {
        var model = await MessagePackSerializer.DeserializeAsync<UploadFileRequestModel>(request.Body);

        if (model.OriginalFileSize.GetLengthInMegabytes() > 50)
        {
            return Results.BadRequest("File size cannot be more than 50 Mb");
        }
        
        var internalToken = await filesService.UploadFile(model);

        return Results.Ok(new UploadFileResponseModel { Token = internalToken });
    })
    .WithName("SaveFile")
    .Produces(StatusCodes.Status200OK);

app.Run();

Type GetFileStorageRepositoryType(IConfigurationSection configurationSection)
{
    Enum.TryParse(configurationSection.GetSection("Type").Value, out StorageType storage);

    return storage switch
    {
        StorageType.External => typeof(ExternalStorageRepository),
        StorageType.Filesystem => typeof(FileSystemStorageRepository),
        StorageType.GridFS => typeof(GridFsStorageRepository),
        _ => throw new ArgumentOutOfRangeException(nameof(storage), storage,
            "Storage type is not specified in settings"),
    };
}