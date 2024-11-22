var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.AddControllersWithViews(); // Adds MVC support.
builder.Services.AddHttpClient();          // Configures HttpClient services.

var app = builder.Build();

// Configure Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Use custom error page in production.
    app.UseHsts();                          // Enforce HSTS for production.
}

app.UseHttpsRedirection();   // Redirect HTTP requests to HTTPS.
app.UseStaticFiles();        // Serve static files like CSS, JS, and images.

app.UseRouting();            // Enable routing middleware.
app.UseAuthorization();      // Enable authorization middleware.

// Map default route for MVC.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run(); // Starts the application.
