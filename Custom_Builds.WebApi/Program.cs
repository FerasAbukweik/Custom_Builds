using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.ExtensionMethods;
using Custom_Builds.Infrastructure.BackgroundServices;
using Custom_Builds.Infrastructure.DBcontext;
using Custom_Builds.Infrastructure.ExtensionMethods;
using custom_Peripherals.Hub;
using custom_Peripherals.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
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
                // Authorization only supplied when generating new tokens via UseAutoRegenerateTokens middleware
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    return Task.CompletedTask;
                }

                if(context.Request.Cookies.TryGetValue("AccessToken" , out string? token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    })
    .AddCookie("Identity.Application");

// adding database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default") ,
    x=>x.MigrationsAssembly("Custom_Builds.Infrastructure"))
);

// add identity services and store users,roles in DBF
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    // user password attributes
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredUniqueChars = 1;
})
.AddRoles<ApplicationRole>()
// decide who is the DB
.AddEntityFrameworkStores<ApplicationDbContext>()
// generate identity token based on Identity information
.AddDefaultTokenProviders()
// decide who is user and where to store it
.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
// decide who is role and where to store it
.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
.AddSignInManager<SignInManager<ApplicationUser>>();


builder.Services.addRepositories();
builder.Services.addApplicationServices();

// Background Services
builder.Services.AddHostedService<RemoveExpiredRefreshTokens>();


// add policy to allow external front end to access the APIs
builder.Services.AddCors(Options =>
{
    Options.AddPolicy("AllowExternalFrontEnd", policy => 
    {
        policy
        .WithOrigins(["http://localhost:4200"])
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
app.UseRouting();
app.UseCors("AllowExternalFrontEnd");
app.UseAutoRegenerateTokens();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();