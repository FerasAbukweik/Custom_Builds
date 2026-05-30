using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Custom_Builds.Infrastructure.ExtensionMethods
{
    public static class DependancyInjection
    {
        public static IServiceCollection addRepositories(this IServiceCollection services)
        {
            services.AddScoped<IChatGroupRepository, ChatGroupRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ICustomBuildRepository, CustomBuildRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IModificationsRepository, ModificationsRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
