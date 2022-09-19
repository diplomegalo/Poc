const string Recipes = "Recipes";
const string Nutrition = "Nutrition";

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpClient(Recipes, (_, client) => client.BaseAddress = new Uri("https://localhost:7285/graphql/"));
builder.Services
    .AddHttpClient(Nutrition, (_, client) => client.BaseAddress = new Uri("https://localhost:7291/graphql/"));

builder.Services
    .AddGraphQLServer()
    .AddRemoteSchema(Recipes)
    .AddRemoteSchema(Nutrition);

var app = builder.Build();

app.MapGraphQL();

app.Run();
