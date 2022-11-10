using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add session
builder.Services
    .AddSession(options => options.Cookie.IsEssential = true);

// Add services to the container.
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        // Infra configuration
        builder.Configuration.Bind("AzureAD", options);

        // Identity request options for hybrid SPA.
        options.ResponseType = OpenIdConnectResponseType.CodeIdToken; // ask for authorization code and id token
        options.Scope.Add("user.read");
        options.Scope.Add("offline_access");

        options.Events.OnAuthorizationCodeReceived = async context =>
        {
            var confidentialApplicationOption = new ConfidentialClientApplicationOptions();
            builder.Configuration.Bind("ConfidentialClientApplication", confidentialApplicationOption);
            
            var app = ConfidentialClientApplicationBuilder.Create(options.ClientId)
                .WithClientSecret(options.ClientSecret)
                .WithRedirectUri("https://localhost:7001/signin-oidc")
                .Build();
            var result = await app.AcquireTokenByAuthorizationCode(options.Scope, context.ProtocolMessage.Code)
                .WithAuthority(AzureCloudInstance.AzurePublic, options.TenantId)
                .ExecuteAsync();
            context.HandleCodeRedemption(result.AccessToken, result.IdToken);
        };
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

app.MapWhen(context => context.Request.Path.Value != null && !context.Request.Path.Value.StartsWith("/api"),
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