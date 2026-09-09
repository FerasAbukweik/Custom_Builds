using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class ClaudinaryService: IClaudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<ClaudinaryService> _logger;
    public ClaudinaryService(IConfiguration configuration,
        ILogger<ClaudinaryService> logger)
    {
        _logger = logger;
        
        var account = new Account(
            configuration["CloudinarySettings:CloudName"], 
            configuration["CloudinarySettings:ApiKey"],
            configuration["CloudinarySettings:ApiSecret"]);
        
        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<ImageUploadResult> Upload(IFormFile image)
    {
        if (image.Length == 0) return new ImageUploadResult();
        
        await using var stream = image.OpenReadStream();
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(image.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill"),
            Folder = "Custom_Builds"
        };
        
        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
        {
            _logger.LogError("{serviceName}.{methodName} failed uploading image\nErrors: {errors}",
                nameof(ClaudinaryService), nameof(Upload), result.Error.Message);
        }
        else
        {
            _logger.LogError("{serviceName}.{methodName} image was uploaded with publicId of {publicId}",
                nameof(ClaudinaryService), nameof(Upload), result.PublicId);
        }

        return result;
    }

    public async Task<DeletionResult> Delete(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deletionParams);

        if (result.Error != null)
        {
            _logger.LogError("{serviceName}.{methodName} failed deleting image\nErrors: {errors}",
                nameof(ClaudinaryService), nameof(Delete), result.Error.Message);
        }
        else
        {
            _logger.LogError("{serviceName}.{methodName} image with public id of {publicId} was deleted",
                nameof(ClaudinaryService), nameof(Delete), publicId);
        }
        
        return result;
    }
}