using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Custom_Builds.Infrastructure.BackgroundServices;
using Custom_Builds.Infrastructure.Repositories;
using Custom_Builds.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Custom_Builds.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // add services
            services.AddScoped<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IModificationsService, ModificationsService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomBuildService, CustomBuildService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IChatGroupService, ChatGroupService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            
            
            // add repositories
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
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
            
            // Background Services
            services.AddHostedService<RemoveExpiredRefreshTokens>();

            return services;
        }
    }
}
