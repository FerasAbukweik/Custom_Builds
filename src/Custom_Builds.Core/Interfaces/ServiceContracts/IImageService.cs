using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IImageService
{
    Task<Result<IReadOnlyList<Image>>> AddRange(IFormFile[] files,Guid? productId = null, CancellationToken cancellationToken = default);
    
    Task<Result<Image>> Add(IFormFile file, CancellationToken cancellationToken = default);
}