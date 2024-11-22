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
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false; // Disable 'Server' header
});
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
    corsBuilder.WithOrigins("https://localhost:7053", "http://localhost:5071") // Allow requests from a specific origin.
               .AllowAnyMethod()
               .AllowAnyHeader());

// Map routes and endpoints.


app.Use(async (context, next) =>
{
    // Remove all response headers
    context.Response.Headers.Remove("Strict-Transport-Security");
    context.Response.Headers.Remove("X-Frame-Options");
    context.Response.Headers.Remove("Access-Control-Allow-Origin");
    context.Response.Headers.Remove("Content-Type");

    await next();
});


app.MapControllers();

// Run the app.
app.Run();
