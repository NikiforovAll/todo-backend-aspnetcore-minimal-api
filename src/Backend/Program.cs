using Backend.Features;
using Carter;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddDbContext<TodoContext>(o =>
    o.UseInMemoryDatabase("todos"));
services.AddCarter();
services.AddCors(options => {
    options.AddDefaultPolicy(b => b
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});
var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.MapCarter();
app.Run();
