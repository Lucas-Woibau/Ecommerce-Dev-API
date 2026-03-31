using EcommerceDev.Infrastructure;
using EcommerceDev.Application;
using EcommerceDev.Core;
using Hangfire;
using EcommerceDev.Infrastructure.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddCore();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var recurringJobs = scope.ServiceProvider.GetService<IRecurringJobManager>();

    recurringJobs.AddOrUpdate<CancelExpiredOrdersJob>("expire-orders",
        job => job.ExecuteAsync(), Cron.Daily);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "ECommerceDev API Background Jobs",
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
