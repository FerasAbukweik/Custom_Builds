using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.CookieServices;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.ICustomBuildServices;
using Custom_Builds.Core.ServiceContracts.IJWTServices;
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
using Custom_Builds.Core.Services.CookiesServices;
using Custom_Builds.Core.Services.CustomBuildServices;
using Custom_Builds.Core.Services.JWTServices;
using Custom_Builds.Core.Services.ModificationServices;
using Custom_Builds.Core.Services.OrderServices;
using Custom_Builds.Core.Services.PartServices;
using Custom_Builds.Core.Services.ProductServices;
using Custom_Builds.Core.Services.RefreshTokenServices;
using Custom_Builds.Core.Services.SectionServices;
using Custom_Builds.Core.Utils;
using Custom_Builds.Infrastructure.DBcontext;
using Custom_Builds.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// mapping controllers
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    // global [authorize]
    options.Filters.Add(new AuthorizeFilter(policy));
});

//so scalar can find the controllers and actions
builder.Services.AddOpenApi();

// implementing serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration configuration) =>
{
    configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(service);
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),

            ValidateLifetime = true,
        };

        options.Events = new JwtBearerEvents()
        {
            // get access token from cookies
            OnMessageReceived = context =>
            {
                if(context.Request.Cookies.TryGetValue("AccessToken" , out string? token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            },


            // try remake new access and refresh Tokens
            OnAuthenticationFailed = async context =>
            {
                if (!context.Request.Cookies.TryGetValue("AccessToken", out string? token))
                {
                    return;
                }

                IJWTService jwtService = context.HttpContext.RequestServices.GetRequiredService<IJWTService>();
                IAddCookieService addCookieService = context.HttpContext.RequestServices.GetRequiredService<IAddCookieService>();

                var tokens = await jwtService.GenerateNewAccessAndRefreshTokensAsync(context.Request);

                if (!tokens.IsSuccess)
                {
                    return;
                }

                addCookieService.Add("AccessToken",
                    tokens.Value!.AccessToken, double.Parse(builder.Configuration["JWT:RefreshTokenLife"]!));
                addCookieService.Add("RefreshToken",
                    tokens.Value!.RefreshToken, double.Parse(builder.Configuration["JWT:RefreshTokenLife"]!));


                var principal = jwtService.GetPrincipal();

                if (!principal.IsSuccess)
                {
                    return;
                }

                // update the principal with new principal
                context.Principal = principal.Value; 
                context.Success();
            }
        };
    });

// adding database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default") ,
    x=>x.MigrationsAssembly("Custom_Builds.Infrastructure"))
);

// add identity services and store users,roles in DB
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
    // user password attributes
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredUniqueChars = 1;
})
// decide who is the DB
.AddEntityFrameworkStores<ApplicationDbContext>()
// generate identity token based on Identity information
.AddDefaultTokenProviders()
// decide who is user and where to store it
.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
// decide who is role and where to store it
.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();


//DI
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPartRepository, PartRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<IModificationsRepository, ModificationsRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomBuildRepository, CustomBuildRepository>();
builder.Services.AddScoped<IGetPartService, GetPartService>();
builder.Services.AddScoped<IAddPartService, AddPartService>();
builder.Services.AddScoped<IEditPartService, EditPartService>();
builder.Services.AddScoped<IRemovePartService, RemovePartService>();
builder.Services.AddScoped<IGetSectionService, GetSectionService>();
builder.Services.AddScoped<IAddSectionService, AddSectionService>();
builder.Services.AddScoped<IEditSectionService, EditSectionService>();
builder.Services.AddScoped<IRemoveSectionService, RemoveSectionService>();
builder.Services.AddScoped<IGetModificationService, GetModificationService>();
builder.Services.AddScoped<IAddModificationService, AddModificationService>();
builder.Services.AddScoped<IEditModificationService, EditModificationService>();
builder.Services.AddScoped<IRemoveModificationService, RemoveModificationService>();
builder.Services.AddScoped<IGetCartItemService, GetCartItemService>();
builder.Services.AddScoped<IAddCartItemService, AddCartItemService>();
builder.Services.AddScoped<IEditCartItemService, EditCartItemService>();
builder.Services.AddScoped<IRemoveCartItemService, RemoveCartItemService>();
builder.Services.AddScoped<IGetOrderService, GetOrderService>();
builder.Services.AddScoped<IAddOrderService, AddOrderService>();
builder.Services.AddScoped<IEditOrderService, EditOrderService>();
builder.Services.AddScoped<IRemoveOrderService, RemoveOrderService>();
builder.Services.AddScoped<IRemoveRefreshTokenService, RemoveRefreshTokenService>();
builder.Services.AddScoped<IGetProductService, GetProductService>();
builder.Services.AddScoped<IAddProductService, AddProductService>();
builder.Services.AddScoped<IEditProductService, EditProductService>();
builder.Services.AddScoped<IRemoveProductService, RemoveProductService>();
builder.Services.AddScoped<IGetCustomBuildService, GetCustomBuildService>();
builder.Services.AddScoped<IAddCustomBuildService, AddCustomBuildService>();
builder.Services.AddScoped<IEditCustomBuildService, EditCustomBuildService>();
builder.Services.AddScoped<IRemoveCustomBuildService, RemoveCustomBuildService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDeleteCurrentUserService, IDeleteCurrentUserService>();
builder.Services.AddScoped<ILoginAccountService, LoginAccountService>();
builder.Services.AddScoped<IRegisterAccountService, RegisterAccountService>();
builder.Services.AddScoped<ILogoutAccountService, LogoutAccountService>();
builder.Services.AddScoped<IDeleteCookieService, DeleteCookieService>();
builder.Services.AddScoped<IAddCookieService , AddCookieService>();
builder.Services.AddScoped<IGetCookieService , GetCookieService>();
builder.Services.AddScoped<IGenerateRefreshTokenService , GenerateRefreshTokenService>();
builder.Services.AddScoped<IGetRefreshTokenService, GetRefreshTokenService>();


builder.Services.AddCors(Options =>
{
    Options.AddPolicy("AllowExternalFrontEnd", policy => 
    {
        policy
        .WithOrigins()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();


    //so scalar can find the controllers and actions
    app.MapOpenApi();
    // use scalar
    app.MapScalarApiReference();
}


app.UseGlobalExceptionMiddleware();
app.UseStaticFiles();
app.UseCors("AllowExternalFrontEnd");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();