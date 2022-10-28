using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAuthentication()
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Controllers
builder.Services.AddControllersWithViews();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowAnyHeader();
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapGet("/plop", (context) =>
{
    var result = context.User.Identity?.IsAuthenticated ?? false
        ? "L'utilisateur est authentifié"
        : "L'utilisateur n'est pas authentifié";
    
    return context.Response.WriteAsync(result);
});

app.MapFallbackToFile("index.html");

app.UseSpa(spa =>
    spa.UseProxyToSpaDevelopmentServer(new Uri("http://localhost:8080/")));

app.Run();