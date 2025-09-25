using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskService.DAL;
using TaskService.Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepository>();
builder.Services.AddSingleton<TaskService.Logic.TaskService>();
builder.Services.AddSingleton<TaskService.Logic.ProjectService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();