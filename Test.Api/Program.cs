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
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Negotiate.NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate(); // Use Negotiate Authentication
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false; // Disable 'Server' header
});
builder.Services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
// Enable response compression.
//builder.Services.AddResponseCompression();

// Build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    var handler = new HttpClientHandler()
    {
        // Disable SSL certificate validation (for development/testing)
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    var client = new HttpClient(handler);

    var response = await client.GetAsync("https://example.com");

}
else if (app.Environment.IsProduction())
{
    //app.UseHttpsRedirection();
}

// Add custom middleware.
app.UseMiddleware<ExceptionMiddelware>();

// Enable response compression middleware.
//app.UseResponseCompression();



// Configure CORS policy.
app.UseCors(corsBuilder =>
    corsBuilder.WithOrigins("https://localhost:7053", "http://localhost:5071", "http://localhost:4200") // Allow requests from a specific origin.
                
                .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               );

// Map routes and endpoints.
app.UseRouting();
app.UseHsts(); // HTTP Strict Transport Security
app.UseXContentTypeOptions(); // Prevents browsers from interpreting files as a different MIME type
app.UseReferrerPolicy(policy => policy.NoReferrer()); // Prevents sending the Referer header
app.UseXXssProtection(options => options.EnabledWithBlockMode()); // Enable XSS protection and block suspicious content
app.UseXfo(options => options.Deny()); // Prevents the site from being framed




app.MapControllers();

// Run the app.
app.Run();
