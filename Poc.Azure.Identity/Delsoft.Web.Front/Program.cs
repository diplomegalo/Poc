using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Controllers
builder.Services.AddControllers();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Welcome !");
app.MapGet("/plop", [Authorize](context) =>
{
    var result = context.User.Identity?.IsAuthenticated ?? false
        ? "L'utilisateur est authentifié"
        : "L'utilisateur n'est pas authentifié";
    
    return context.Response.WriteAsync(result);
});

app.MapControllers();

app.Run();