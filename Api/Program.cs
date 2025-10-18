using Microsoft.AspNetCore.Builder;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskService.DAL;
using TaskService.Logic;
using TaskService.Application.Http;
using Domain.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// DI для TaskService
builder.Services.AddSingleton<TaskService.DAL.IProjectRepository, TaskService.DAL.InMemoryProjectRepository>();
builder.Services.AddSingleton<TaskService.DAL.ITaskRepository>(sp =>
    new TaskService.DAL.InMemoryTaskRepository(sp.GetRequiredService<TaskService.DAL.IProjectRepository>()));
builder.Services.AddSingleton<TaskService.Logic.TaskService>();
builder.Services.AddSingleton<TaskService.Logic.ProjectService>();

// HttpService registration (TaskService.Application.Http.HttpService)
builder.Services.AddHttpClient<TaskService.Application.Http.IHttpService, TaskService.Application.Http.HttpService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
//     app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// TraceId middleware: read X-Trace-Id from incoming request and set TraceId.Current
app.Use(async (context, next) =>
{
    var trace = context.Request.Headers["X-Trace-Id"].FirstOrDefault();
    if (!string.IsNullOrEmpty(trace))
    {
        TraceId.SetTraceId(trace);
    }

    try
    {
        await next();
    }
    finally
    {
        TraceId.Clear();
    }
});

app.UseAuthorization();

app.MapControllers();

app.Run();