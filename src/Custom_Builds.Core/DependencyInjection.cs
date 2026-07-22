using Microsoft.Extensions.DependencyInjection;

namespace Custom_Builds.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection service)
    {
        return service;
    }
}