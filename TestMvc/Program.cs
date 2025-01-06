var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false; // Disable the Server header
});
// Configure Services
builder.Services.AddControllersWithViews(); // Adds MVC support.
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Use custom error page in production.
    app.UseHsts();                          // Enforce HSTS for production.
}
else
{
    var handler = new HttpClientHandler()
    {
        // Disable SSL certificate validation (for development/testing)
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    var client = new HttpClient(handler);

    var response = await client.GetAsync("https://example.com");

}
//app.UseHttpsRedirection();   // Redirect HTTP requests to HTTPS.
app.UseStaticFiles();        // Serve static files like CSS, JS, and images.

app.UseRouting();            // Enable routing middleware.
app.UseHsts(); // HTTP Strict Transport Security
app.UseXContentTypeOptions(); // Prevents browsers from interpreting files as a different MIME type
app.UseReferrerPolicy(policy => policy.NoReferrer()); // Prevents sending the Referer header
app.UseXXssProtection(options => options.EnabledWithBlockMode()); // Enable XSS protection and block suspicious content
app.UseXfo(options => options.Deny()); // Prevents the site from being framed



// Map default route for MVC.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run(); // Starts the application.
