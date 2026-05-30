using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.CookieServices;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;
using Custom_Builds.Core.ServiceContracts.IChatGroupServices;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.ICustomBuildServices;
using Custom_Builds.Core.ServiceContracts.IJWTServices;
using Custom_Builds.Core.ServiceContracts.IMessageServices;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.IOrderServices;
using Custom_Builds.Core.ServiceContracts.IPartServices;
using Custom_Builds.Core.ServiceContracts.IProductServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Custom_Builds.Core.ServiceContracts.ISectionServices;
using Custom_Builds.Core.ServiceContracts.ModificationServices;
using Custom_Builds.Core.ServiceContracts.OrderServices;
using Custom_Builds.Core.ServiceContracts.PartServices;
using Custom_Builds.Core.Services.AccountServices;
using Custom_Builds.Core.Services.CartItemServices;
using Custom_Builds.Core.Services.ChatGroupServices;
using Custom_Builds.Core.Services.CookiesServices;
using Custom_Builds.Core.Services.CurrUserServices;
using Custom_Builds.Core.Services.CustomBuildServices;
using Custom_Builds.Core.Services.JWTServices;
using Custom_Builds.Core.Services.MessageServices;
using Custom_Builds.Core.Services.ModificationServices;
using Custom_Builds.Core.Services.OrderServices;
using Custom_Builds.Core.Services.PartServices;
using Custom_Builds.Core.Services.ProductServices;
using Custom_Builds.Core.Services.RefreshTokenServices;
using Custom_Builds.Core.Services.SectionServices;
using Microsoft.Extensions.DependencyInjection;

namespace Custom_Builds.Core.ExtensionMethods
{
    public static class DependancyInjection
    {
        public static IServiceCollection addApplicationServices(this IServiceCollection services)
        {
            // JWTservices 
            services.AddScoped<IJWTService, JWTService>();

            // RefreshTokensservices + Reposotory
            services.AddScoped<IRemoveRefreshTokenService, RemoveRefreshTokenService>();
            services.AddScoped<IGenerateRefreshTokenService, GenerateRefreshTokenService>();
            services.AddScoped<IGetRefreshTokenService, GetRefreshTokenService>();

            // Part services + Reposotory
            services.AddScoped<IGetPartService, GetPartService>();
            services.AddScoped<IAddPartService, AddPartService>();
            services.AddScoped<IEditPartService, EditPartService>();
            services.AddScoped<IRemovePartService, RemovePartService>();

            // Section services + Reposotory
            services.AddScoped<IGetSectionService, GetSectionService>();
            services.AddScoped<IAddSectionService, AddSectionService>();
            services.AddScoped<IEditSectionService, EditSectionService>();
            services.AddScoped<IRemoveSectionService, RemoveSectionService>();

            // Modification services + Reposotory
            services.AddScoped<IAddModificationService, AddModificationService>();
            services.AddScoped<IEditModificationService, EditModificationService>();
            services.AddScoped<IRemoveModificationService, RemoveModificationService>();
            services.AddScoped<IGetModificationService, GetModificationService>();

            // Cart services + Reposotory
            services.AddScoped<IGetCartItemService, GetCartItemService>();
            services.AddScoped<IAddCartItemService, AddCartItemService>();
            services.AddScoped<IRemoveCartItemService, RemoveCartItemService>();
            services.AddScoped<IUpdateCartItemService, UpdateCartItemService>();

            // Order services + Reposotory
            services.AddScoped<IGetOrderService, GetOrderService>();
            services.AddScoped<IAddOrderService, AddOrderService>();
            services.AddScoped<IEditOrderService, EditOrderService>();
            services.AddScoped<IRemoveOrderService, RemoveOrderService>();

            // Product services + Reposotory
            services.AddScoped<IAddProductService, AddProductService>();
            services.AddScoped<IGetProductService, GetProductService>();
            services.AddScoped<IEditProductService, EditProductService>();
            services.AddScoped<IRemoveProductService, RemoveProductService>();

            // CustomBuild services + Repository
            services.AddScoped<IGetCustomBuildService, GetCustomBuildService>();
            services.AddScoped<IAddCustomBuildService, AddCustomBuildService>();
            services.AddScoped<IEditCustomBuildService, EditCustomBuildService>();
            services.AddScoped<IRemoveCustomBuildService, RemoveCustomBuildService>();

            // Account services
            services.AddScoped<IDeleteUserService, DeleteUserService>();
            services.AddScoped<ILoginAccountService, LoginAccountService>();
            services.AddScoped<IRegisterAccountService, RegisterAccountService>();
            services.AddScoped<ILogoutAccountService, LogoutAccountService>();

            // Cookie services
            services.AddScoped<IDeleteCookieService, DeleteCookieService>();
            services.AddScoped<IAddCookieService, AddCookieService>();
            services.AddScoped<IGetCookieService, GetCookieService>();

            // Message services + repository
            services.AddScoped<IAddMessageService, AddMessageService>();
            services.AddScoped<IGetMessageService, GetMessageService>();

            // Chat group services + repository
            services.AddScoped<IAddChatGroupService, AddChatGroupService>();
            services.AddScoped<IGetChatGroupService, GetChatGroupService>();

            // Current User services
            services.AddScoped<IGetCurrUserService, GetCurrUserService>();

            // so we can access http context in services
            services.AddHttpContextAccessor();

            // add signalR service
            services.AddSignalR(options => options.EnableDetailedErrors = true);


            return services;
        }
    }
}
