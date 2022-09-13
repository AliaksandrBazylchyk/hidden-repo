using MovieService.Jobs;
using Quartz;
using ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCore();

builder.Services.AddRabbitMq();

//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddOptions();

builder.Services.AddSQLDatabase("Host=pgdb;Port=5432;Database=MovieBusinessDB;Username=root;Password=root");

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

app.UseCore();
app.UseRabbitMq();
app.UseSQLDatabase();

app.MapControllers();

app.Logger.LogInformation("Application started.");

app.Run();
