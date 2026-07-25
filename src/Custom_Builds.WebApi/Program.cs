using Custom_Builds.Core;
using Custom_Builds.Infrastructure;
using custom_Peripherals;
using custom_Peripherals.Hub;
using custom_Peripherals.MiddleWare;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// implementing serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration configuration) =>
{
    configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(service);
});


// add services
builder.Services
    .AddCore()
    .AddInfrastructure()
    .AddWepApi(builder.Configuration);


var app = builder.Build();

// use https
app.UseHsts();
app.UseHttpsRedirection();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();