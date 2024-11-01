using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Microsoft.OpenApi.Models; 
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<WebSocketService, WebSocketService>();

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // Include XML comments if enabled in the project file
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// Configure CORS policy before authorization
app.UseCors("AllowAllOrigins");

// Ensure WebSockets middleware is registered
app.UseWebSockets(); // Make sure this is before app.UseAuthorization()

app.UseAuthorization();

app.MapControllers();

app.Run();
