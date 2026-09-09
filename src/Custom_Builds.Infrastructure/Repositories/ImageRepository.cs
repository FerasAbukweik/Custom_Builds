using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;

namespace Custom_Builds.Infrastructure.Repositories;

public class ImageRepository(ApplicationDbContext dbContext) : IImageRepository
{
    public void Add(Image image)
    {
        dbContext.Images.Add(image);
    }

    public void AddRange(Image[] images)
    {
        dbContext.Images.AddRange(images);
    }

    public async Task<Image?> RemoveById(string imageId)
    {
        var toRemove = await dbContext.Images.FindAsync(imageId);

        if (toRemove == null) return null;
        
        dbContext.Images.Remove(toRemove);
        
        return  toRemove;
    }

    public async Task<Image?> RemoveByPublicId(string publicId)
    {
        var toRemove = await dbContext.Images.SingleOrDefaultAsync(i => i.PublicId == publicId);

        if (toRemove == null) return null;
        
        dbContext.Images.Remove(toRemove);
        
        return toRemove;
    }


    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return (await dbContext.SaveChangesAsync(cancellationToken)) > 0;
    }
}