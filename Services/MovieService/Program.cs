using Commands;
using Core.Aggregate;
using Core.Outbox;
using Core.RabbitMq;
using Core.Repository;
using DatabaseExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MovieService.Jobs;
using Quartz;
using Queries;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ICommandBus, CommandBus>();
builder.Services.AddScoped<IQueryBus, QueryBus>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IOutboxStore<>), typeof(OutboxStore<>));
builder.Services.AddScoped(typeof(IAggregate<,>), typeof(Aggregate<,>));
builder.Services.AddSingleton(typeof(IRabbitMqCQRSHelper), typeof(RabbitMqCQRSHelper));

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddOptions();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql("Host=pgdb;Port=5432;Database=MovieBusinessDB;Username=root;Password=root"));

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("MovieOutboxWatchJob");
    q.AddJob<MovieOutboxWatchJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts.ForJob(jobKey).WithCronSchedule("0/5 * * * * ?"));

});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("Application started.");

app.Run();
