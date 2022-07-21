using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Poc.GraphQL.Web.GraphQL;
using Poc.GraphQL.Web.Repositories;
using Poc.GraphQL.Web.Repositories.Abstractions;
using Poc.GraphQL.Web.Repositories.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<PocGraphQLContext>(options => options.UseInMemoryDatabase("PocGraphQL"));
builder.Services.BuildServiceProvider().GetRequiredService<PocGraphQLContext>().Database.EnsureCreated();

// Repositories
builder.Services.AddScoped<IContractRepository, ContractRepository>();

// GraphQL
builder.Services
    .AddGraphQL(qlBuilder => qlBuilder
        .AddHttpMiddleware<ISchema>()
        .AddSelfActivatingSchema<PocApplicationSchema>()
        .AddSystemTextJson()
        .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = builder.Environment.IsDevelopment()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // GraphQL client Altair
    app.UseGraphQLAltair();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseGraphQL<ISchema>();

app.Run();