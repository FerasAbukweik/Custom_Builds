using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Custom_Builds.Infrastructure.BackgroundServices;
using Custom_Builds.Infrastructure.DBcontext;
using Custom_Builds.Infrastructure.Repositories;
using Custom_Builds.Infrastructure.Services;
using HR_System.Core.Interfaces.ServiceContracts;
using HR_System.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Custom_Builds.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // adding database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("default")
                )
            );
            
            // add redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = configuration["Redis:InstanceName"];
            });
        
            services.AddSingleton<IConnectionMultiplexer>(sp => 
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis") ??  "localhost:6379"));
            
            // add services
            services.AddScoped<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomBuildService, CustomBuildService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IChatGroupService, ChatGroupService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IClaudinaryService, ClaudinaryService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRedisService, RedisService>();
            
            // cached
            services.AddKeyedScoped<IPartService, PartService>("inner");
            services.AddScoped<IPartService, CachedPartService>();
            services.AddKeyedScoped<ISectionService, SectionService>("inner");
            services.AddScoped<ISectionService, CachedSectionService>();
            services.AddKeyedScoped<IModificationsService, ModificationsService>("inner");
            services.AddScoped<IModificationsService, CachedModificationsService>();
            
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
            services.AddScoped<IImageRepository, ImageRepository>();
            
            // Background Services
            services.AddHostedService<RemoveExpiredRefreshTokens>();

            return services;
        }
    }
}
