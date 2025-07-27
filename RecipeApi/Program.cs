using Microsoft.EntityFrameworkCore;
using RecipeApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext for MySQL with connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Enable CORS policy to allow requests from your React front end
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keeps property names as PascalCase
        options.JsonSerializerOptions.WriteIndented = true;       // Pretty-print JSON
    });

var app = builder.Build();

app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();
