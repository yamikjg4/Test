using System.Text.Json.Serialization;
using Utility;
using Utility.Middelware;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDependencies();
builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();

builder.Services.AddSwaggerGen();
builder.Services.AddResponseCompression();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddelware>();
app.UseResponseCompression();
app.UseAuthorization();
app.UseCors(builder =>
        builder.WithOrigins("https://localhost:7053")  // Allow requests from this origin
               .AllowAnyMethod()
               .AllowAnyHeader()
    );
app.MapControllers();

app.Run();
