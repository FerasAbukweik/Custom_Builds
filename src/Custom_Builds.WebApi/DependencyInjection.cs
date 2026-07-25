using System.Text;
using Custom_Builds.Core.Constants;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Custom_Builds.Infrastructure.DBcontext;
using custom_Peripherals.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace custom_Peripherals;

public static class WepApiDependencyInjection
{
    public static IServiceCollection AddWepApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),

                    ValidateLifetime = true,
                };

                options.Events = new JwtBearerEvents()
                {
                    // get access token from cookies
                    OnMessageReceived = context =>
                    {
                        var cookieKeys = context.HttpContext.RequestServices.GetRequiredService<IOptions<CookieKeys>>();
                        
                        if (context.Request.Cookies.TryGetValue(cookieKeys.Value.AccessToken, out var token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        
        // adding database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("default")
            )
        );


        services.Configure<CookieKeys>(configuration.GetSection("CookieKeys"));
        
        // add identity
        services.AddIdentityCore<ApplicationUser>(options =>
            {
                // user password attributes
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        
        // add policy to allow external front end to access the APIs
        services.AddCors(Options =>
        {
            Options.AddPolicy("AllowExternalFrontEnd", policy => 
            {
                policy
                    .WithOrigins("https://localhost:4000", "https://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        
        // services
        services.AddScoped<ICookieService, CookiesService>();
        
        // add http context accessor
        services.AddHttpContextAccessor();

        // add signalR service
        services.AddSignalR(options => options.EnableDetailedErrors = true);
        
        services.AddOpenApi();
        services.AddControllers();
        
        

        return services;
    }
}