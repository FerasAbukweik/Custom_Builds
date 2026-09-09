using Custom_Builds.Core.Domain.Entities;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts;

public interface IImageRepository
{
    void Add(Image image);
    void AddRange(Image[] images);
    Task<Image?> RemoveById(string imageId);
    Task<Image?> RemoveByPublicId(string publicId);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}