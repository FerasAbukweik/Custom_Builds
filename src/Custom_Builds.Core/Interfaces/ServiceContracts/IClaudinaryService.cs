using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IClaudinaryService
{
    Task<ImageUploadResult> Upload(IFormFile image);
    Task<DeletionResult> Delete(string publicId);
}