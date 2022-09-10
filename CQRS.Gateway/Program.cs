using CQRS.Gateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", true, true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddSwaggerForOcelot(configuration)
    .AddOcelot(configuration)
    .AddConsul()
    .AddConfigStoredInConsul()    ;
builder.Services.AddControllers();
builder.Services.AllowAnyCors();

var app = builder.Build();

app.UseCors("AllowOrigins");

app.UseStaticFiles();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseOcelot().Wait();

app.Run();