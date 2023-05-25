using InternalMessagingSystem.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "InternalMessagingSystem",
        Description = "A simple internal messaging system.",
        Contact = new OpenApiContact
        { Name = "Yan LU", Email = "luyansasc@gmail.com" }
    });
    var filePath = Path.Combine(AppContext.BaseDirectory, "InternalMessagingSystem.xml");
    c.IncludeXmlComments(filePath);
});

// Adds internal messaging system services
builder.Services.AddInternalMessagingSystem();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();