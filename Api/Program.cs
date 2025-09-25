using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// DI для TaskService
builder.Services.AddSingleton<TaskService.DAL.IProjectRepository, TaskService.DAL.InMemoryProjectRepository>();
builder.Services.AddSingleton<TaskService.DAL.ITaskRepository>(sp =>
    new TaskService.DAL.InMemoryTaskRepository(sp.GetRequiredService<TaskService.DAL.IProjectRepository>()));
builder.Services.AddSingleton<TaskService.Logic.TaskService>();
builder.Services.AddSingleton<TaskService.Logic.ProjectService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
//     app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();