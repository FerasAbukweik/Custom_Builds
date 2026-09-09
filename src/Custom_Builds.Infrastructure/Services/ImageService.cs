using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class ImageService(
    IImageRepository imageRepository,
    IClaudinaryService claudinaryService,
    ILogger<ImageService> logger) : IImageService
{
    public async Task<Result<IReadOnlyList<Image>>> AddRange(IFormFile[] files,Guid? productId = null, CancellationToken cancellationToken = default)
    {
        var images = new List<Image>();
        // upload images to cloudinary
        foreach (var image in files)
        {
            var result = await claudinaryService.Upload(image);

            if (result.Error != null) return Result<IReadOnlyList<Image>>.Failure(result.Error.Message);
            
            // save images info
            images.Add(new Image()
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                ProductId = productId
            });
        }
        
        imageRepository.AddRange(images.ToArray());
        
        if (!await imageRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(ImageService), nameof(AddRange));
            return Result<IReadOnlyList<Image>>.Failure("failed saving changes to DB");
        }
        
        return Result<IReadOnlyList<Image>>.Success(images);
    }

    public async Task<Result<Image>> Add(IFormFile file, CancellationToken cancellationToken = default)
    {
        var result = await AddRange([file], null, cancellationToken);

        if (!result.IsSuccess) return result.MapFailure<Image>();
        
        return Result<Image>.Success(result.Value![0]);
    }
}