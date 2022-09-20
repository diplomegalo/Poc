const string Recipes = "recipes";
const string Nutrition = "nutrition";

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpClient(Nutrition, (_, client) => client.BaseAddress = new Uri("https://localhost:7285/graphql/"));
builder.Services
    .AddHttpClient(Recipes, (_, client) => client.BaseAddress = new Uri("https://localhost:7291/graphql/"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
    .AddRemoteSchema(Recipes, ignoreRootTypes: true)
    .AddRemoteSchema(Nutrition, ignoreRootTypes: true)
    .AddTypeExtensionsFromFile("./Stitching.graphql")
    .ModifyRequestOptions(options => options.IncludeExceptionDetails = true);

var app = builder.Build();

app.MapGraphQL();

app.Run();