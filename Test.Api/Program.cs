using System.Text.Json.Serialization;
using Utility;
using Utility.Middelware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDependencies(); // Register custom dependencies.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); // Add JSON enum serialization.

// Add Swagger and memory caching.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// Enable response compression.
builder.Services.AddResponseCompression();

// Build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add custom middleware.
app.UseMiddleware<ExceptionMiddelware>();

// Enable response compression middleware.
app.UseResponseCompression();

// Configure CORS policy.
app.UseCors(corsBuilder =>
    corsBuilder.WithOrigins("https://localhost:7053") // Allow requests from a specific origin.
               .AllowAnyMethod()
               .AllowAnyHeader());

// Map routes and endpoints.
app.UseAuthorization();
app.MapControllers();

// Run the app.
app.Run();
