using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add session
builder.Services
    .AddSession(options => options.Cookie.IsEssential = true);

// Add services to the container.
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(
        options => builder.Configuration.Bind("AzureAD", options),
        options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromSeconds(5);
            options.SlidingExpiration = true;
        });

// Controllers
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();


app.Use(async (context, next) =>
{
    if (context.User.Identity is { IsAuthenticated: false })
    {
        await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
    else
    {
        await next();
    }
});

app.MapWhen(context => context.Request.Path.Value != null 
                       && !context.Request.Path.Value.StartsWith("/api")
                       && !context.Request.Path.Value.StartsWith("/signin-oidc"),
    applicationBuilder => applicationBuilder.UseSpa(
        spa => { spa.UseProxyToSpaDevelopmentServer("http://localhost:8080/"); }));

app.Map("/api/plop", context =>
{
    var result = context.User.Identity?.IsAuthenticated ?? false
        ? "L'utilisateur est authentifié"
        : "L'utilisateur n'est pas authentifié";

    return context.Response.WriteAsync(result);
});

app.Run();