using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.ServiceContracts;
using Custom_Builds.Core.Services;
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
            ValidAudience = builder.Configuration["JWT:Audiance"],
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

                AccessAndRefreshTokenDTO tokens = await jwtService.GenerateNewAccessAndRefreshTokensAsync(context.Request);

                CookiesUtils.AddToCookies(context.Response, "AccessToken",
                    tokens.AccessToken, double.Parse(builder.Configuration["JWT:RefreshTokenLife"]!));
                CookiesUtils.AddToCookies(context.Response, "RefreshToken",
                    tokens.RefreshToken, double.Parse(builder.Configuration["JWT:RefreshTokenLife"]!));


                var principal = jwtService.GetPrincipalFromAccessToken(tokens.AccessToken);
                // update the principal with new principal
                context.Principal = principal; 
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
builder.Services.AddTransient<IJWTService, JWTService>();
builder.Services.AddScoped<IRefreshTokenRepositry, RefreshTokenRepositry>();

builder.Services.AddCors(Options =>
{
    Options.AddPolicy("AllowExternalFrontEnd", policy => 
    {
        policy
        .WithOrigins([""])
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

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
