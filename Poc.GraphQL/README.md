# Create wep api solution

>[!info] Sources  
You can find source of the solution on the [Poc](https://github.com/diplomegalo/Poc) repository on GitHub in the [Poc.GraphQL](https://github.com/diplomegalo/Poc/tree/develop/Poc.GraphQL) directory.

```shell
dotnet new webapi -o Poc.GraphQL/Poc.GraphQL.Web
dotnet new sln -o Poc.GraphQL
dotnet sln add Poc.GraphQL/Poc.GraphQL.Web
```

# Base nuget packages

```shell
dotnet add package GraphQL.Server.All
```

Or more specifics:

```shell
dotnet add package GraphQL.SystemTextJson
dotnet add package GraphQL.MicrosoftDI
dotnet add package GraphQL.Server.Ui.Altair
dotnet add package GraphQL.Server.Transports.AspNetCore
```

# Scalar Type

## Entity definition

Create a new directory `/Entities` and a the class behind which describe a case.

```c#
public class Case  
{  
    public int Id { get; set; }  
    public string? Ref { get; set; }   
}
```

## Type definition

Create new directory `/GraphQL/Types` and add the class behind to define the type case.

```c#
public sealed class CaseType : ObjectGraphType<Case>  
{  
    public CaseType()  
    {        
	    Name = "Case";  
        Description = "A case contains information about the customer.";  
        Field(dossier => dossier.Id).Description("The case unique identifier.");  
        Field(dossier => dossier.Ref).Description("The case business reference.");  
    }
}
```

## Query definition

Add the class `PocApplicationQuery` in the `GraphQL` directory. This class will define the available queries.

```c#
public class PocApplicationQuery : ObjectGraphType  
{  
    public PocApplicationQuery()  
    {        
		Field<ListGraphType<CaseType>>(  
            "dossier",  
            resolve: _ => new List<Case>()  
            {  
                new() { Id = 1, Ref = "plop reference 1" },  
                new() { Id = 2, Ref = "plop reference 2" },  
            });  
    }
}
```

## Schema definition

Finally add the `PocApplicationSchema` in the `GraphQL` directory and set the `Query` property.

```c#
public class PocApplicationSchema : Schema  
{  
    public PocApplicationSchema(IServiceProvider serviceProvider)  
        : base(serviceProvider)  
    {        
	    Query = serviceProvider.GetRequiredService<PocApplicationQuery>();  
    }
}
```

## Use `enum` property

Add the enum type in the `Entities` directory.

```C#
public enum Status  
{  
    New,  
    InProgress,  
    OnHold,  
    Archive  
}
```

Add a new property in `Case` class

```C#
public class Case  
{  
    public int Id { get; set; }  
    public string? Ref { get; set; }

	// enum property
    public Status Status { get; set; }  
}
```

In the `Types` repository add a new type definition for the enumeration.

```C#
public class StatusEnumType : EnumerationGraphType<Status>  
{  
    public StatusEnumType()  
    {        
	    Name = "Status";  
        Description = "The status of the case.";  
    }
}
```

Add field in the `CaseType` class constructor where

- the generic type is the type definition of the enum field,
- the first parameter is the name of the property : `Case.Status`,
- the last parameter is the description.

```C#
public CaseType()  
{  
    Name = "Case";  
    Description = "A case contains information about the customer.";  
    Field(dossier => dossier.Id).Description("The case unique identifier.");  
    Field(dossier => dossier.Ref).Description("The case business reference.");
      
    // Enum type  
    Field<StatusEnumType>("Status", "The status of the case.");  
}
```

## Test

At this step you should be able to test your application by configuring `startup` class following the [[#Configure Startup]] chapter with the minimum configuration.

# Complex Type

## Entity definition

In the `Entities` directory, add a new class `Contract` which describes contracts associated to a case.

```C#  
public class Contract  
{  
    public int Id { get; set; }  
    public string Ref { get; set; }  
    public DateTime CreationDate { get; set; }  
    // Case  
    public int CaseId { get; set; }  
    public Case Case { get; set; }  
}
```

In the `Case` class, add the associated collection of contracts.

```C#
public class Case  
{  
    public int Id { get; set; }  
    public string? Ref { get; set; }  
    public Status Status { get; set; }  
 
	public ICollection<Contract> Contracts { get; set; }  
}

```

>Note that the type of collection is a `ICollection` to be compatible further with EF Core.

## Case repository

Create a new repository to retrieve the query to retrieve all cases.

```
public class FakeCaseRepository : ICaseRepository  
{  
    public IEnumerable<Case> GetAll() =>  
        new List<Case>()  
        {  
            new() { Id = 1, Ref = "plop reference 1", Status = Status.Archive },  
            new() { Id = 2, Ref = "plop reference 2", Status = Status.InProgress }  
        };
}
```

Then use it to resolve query in the `PocApplicationQuery`.

```C#
public class PocApplicationQuery : ObjectGraphType  
{  
    public PocApplicationQuery()  
    {        
	    Field<ListGraphType<CaseType>>(  
            "dossier",  
            resolve: context =>  
            {  
                var repository = context.RequestServices?.GetService<ICaseRepository>() ??  
                       throw new InvalidOperationException(  
                           $"Unable to resolve the {nameof(ICaseRepository)}");  
                return repository.GetAll();  
            });    
    }
}
```

## Contract repository

The repository will be used in the following type definition with the method `GetByCase` that returns contracts associated to a case.

In the `Repositories` directory, first add the `IContractRepository` interface and then the `FakeContractRepository` wich implements the interface and use the `PocGraphQLContext`.

```C#
public interface IContractRepository  
{  
    IEnumerable<Contract> GetByCase(int caseId);  
}
```

```C#
public class FakeContractRepository : IContractRepository  
{  
	public IEnumerable<Contract> GetByCase(int caseId) =>  
	    new List<Contract>()  
	    {  
	        new() { Id = 1, Ref = "Ref1", CreationDate = DateTime.Now },  
	        new() { Id = 2, Ref = "Ref2", CreationDate = DateTime.Now }  
	    }
	    .Where(contract => contract.CaseId == caseId)
	    .ToList(); 
}
```

Then modify the `Startup` class by adding the following code.

```C#
// Repositories  
builder.Services.AddScoped<IContractRepository, FakeContractRepository>();
builder.Services.AddScoped<ICaseRepository, FakeCaseRepository>();
```

## Type definition

In the `GraphQL/Types` directory, add a new class `ContractType` which defines the `ObjectGraphType` for this entity.

```C#
public sealed class ContractType : ObjectGraphType<Contract>  
{  
    public ContractType()  
    {        
	    Field(contract => contract.Id);  
        Field(contract => contract.Ref);  
        Field(contract => contract.CreationDate);  
    }
}
```

Next, adapt the existing `CaseType` class with dependencies on contract.

We use the `IServiceProvider` via the `RequestServices` property of the `context` to resolve the `IContractRepository` instanciation. Find more information about the [Thread safety with scoped services](https://graphql-dotnet.github.io/docs/getting-started/dependency-injection/#thread-safety-with-scoped-services) which explain why use the `RequestServices` property there.

Then the instance is use to retrieve the list of `Contract` associated with a `Case`, based on the `Id` found in the `Source` property of the `context`.

```C#
// Contracts  
Field<ListGraphType<ContractType>>(
	"contracts",  
    resolve: context =>  
    {  
	    // Get repository instance using the service provider
        var contactRepository = context
	        .RequestServices?
	        .GetRequiredService<IContractRepository>() 
		        ?? throw new InvalidOperationException("Unable to retrieve service provider.");

		// Get contract base on the case id
        return contactRepository.GetByCase(context.Source.Id);  
    });
```

## DataLoader

The `Dataloader` is used to increase data fetching performance by:

- batch similar fetch operation together,
- push fetch values in a cache.

### Nuget package

Add new nuget dependencies

```shell
dotnet add package GraphQL.DataLoader
```

### Add service

Add the `AddDataLoader` service (cf. [[#Configure Startup]])

### Enhance repository

First add new method in the `IContractRepository` to allow to request a list of `Contract` based on a list of case identifiers (`IEnumurable<int> caseIds`).

```C#
Task<ILookup<int, Contract>> GetContractsByCaseIdBatchAsync(IEnumerable<int> caseIds);
```

You can notice that the method returns a `ILookup` object which allow to have same keys, instead of the `IDictionary` type.

Then implements the method in the `ContractRepository`.

```C#
public Task<ILookup<int, Contract>> GetContractsByCaseIdBatchAsync(IEnumerable<int> caseIds) =>  
        Task.FromResult(  
            new List<Contract>()  
                {  
                    new() { Id = 1, Ref = "Ref-1", CreationDate = DateTime.Now.AddDays(-1), CaseId = 1 },  
                    new() { Id = 2, Ref = "Ref-2", CreationDate = DateTime.Now.AddDays(-2), CaseId = 2 }  
                }                .Where(contract => caseIds.Contains(contract.CaseId))  
                .ToLookup(contract => contract.Id, contract => contract)); 
```

### Use `DataLoader`

Modify the `CaseType` class to retrieve contracts by using the batch feature of the `DataLoader`.

First add a `IDataLoaderContextAccessor` parameter in the constructor, then update the resolve method by using the `GetOrAddCollectionBatchLoader` method.

```C#
public CaseType(IDataLoaderContextAccessor dataLoaderContextAccessor)  
{  
    Name = "Case";  
    Description = "A case contains information about the customer.";  
    Field(dossier => dossier.Id).Description("The case unique identifier.");  
    Field(dossier => dossier.Ref).Description("The case business reference.");  
  
    // Enum type  
    Field<StatusEnumType>("Status", "The status of the case.");  
  
    // Contracts  
    Field<ListGraphType<ContractType>>(  
        "contracts",  
        resolve: context =>  
        {  
            // Get repository instance  
            var contractRepository = context.RequestServices?.GetRequiredService<IContractRepository>() ??  
                                    throw new InvalidOperationException("Unable to retrieve service provider.");  
              
            // Use data loader to retrieve data  
            return dataLoaderContextAccessor.Context  
                .GetOrAddCollectionBatchLoader<int, Contract>("GetContractsByCaseId", caseIds => contractRepository.GetContractsByCaseIdBatchAsync(caseIds))  
                .LoadAsync(context.Source.Id);  
        });  
}
```

# Configure Startup

Add snippets behind in the `Startup` class.

```c#
// GraphQL  
builder.Services    
.AddGraphQL(qlBuilder => qlBuilder
	// Minimum configuration
	.AddSelfActivatingSchema<PocApplicationSchema>()  
	.AddSystemTextJson()  
  
	// See exception stack trace in the client  
	.AddErrorInfoProvider(options => 
		options.ExposeExceptionStackTrace = builder.Environment.IsDevelopment())  
  
	// Use data loader  
	.AddDataLoader());  

if (app.Environment.IsDevelopment())
{
	app.UseGraphQLAltair();
}

app.UseGraphQL<ISchema>();
```

Now test the appliction by running the command behind to start the webapi and go to GraphQL client by adding `ui/altair` at the end of the base url of your application.

```shell
 dotnet run --project .\Poc.GraphQL\Poc.GraphQL.Web\
```

In the altair web client, add this in the query frame :

```graphql
{
  dossier {
    id,
    ref,
    status
  }
}
```

or with `Contract` results

```graphql
{
  dossier {
    id,
    ref,
    status
    contracts{
      id,
      ref
    }
  }
}

```

## Exception stack trace

The method `.AddErrorInfoProvider` allows to manage the `IErrorInfoProvider` service. In the exemple below, the option `ExposeExceptionStackTrace` is set to `true` when the application run in development. This option allow to render the exception trace in the response when error occurs.

## Middleware

To create a GraphQL middleware, simply use the `.AddHttpMiddleware<ISchema>()` method.

## DataLoader

This require to add nuget package and is only needed when you have complex types and you must increase fetching data performance (which is the case in 99,99999%).

```shell
dotnet add package GraphQL.DataLoader
```

See the [DataLoader](#DataLoader) chapter for more information.

# Use EF Core

## Nuget package

From the path `Poc.GraphQL\Poc.GraphQL.Web`, install the needed EF Core packages with the followin commands.

```cmd
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

## Configure DbContext

In the `program.cs` file, register the `DbContext` with the following code.

```
// DbContext  
builder.Services.AddDbContext<PocGraphQLContext>(options => 
	options.UseInMemoryDatabase("PocGraphQL"));  
```

In the `Repositories/EF` directory, create the `PocGraphQLContext` class as below.

```C#
public sealed class PocGraphQLContext : DbContext  
{  
    public PocGraphQLContext(DbContextOptions<PocGraphQLContext> options)  
        :base(options)  
    {        
	    // Create database and tables
	    Database.EnsureCreated();  
    }    
    
    public DbSet<Case> Cases { get; set; }  
    public DbSet<Contract> Contracts { get; set; }  
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {        
	    base.OnModelCreating(modelBuilder);  
        
        modelBuilder.Entity<Case>().HasData(  
            new Case { Id = 1, Ref = "Ref-case-1", Status = Status.Archive },  
            new Case { Id = 2, Ref = "Ref-case-2", Status = Status.New });  
                
        modelBuilder.Entity<Contract>().HasData(  
            new Contract { 
	            Id = 1, 
	            Ref = "Ref-1", 
	            CreationDate = DateTime.Now.AddDays(-1), 
	            CaseId = 1 
	        },  
            new Contract { 
	            Id = 2, 
	            Ref = "Ref-2", 
	            CreationDate = DateTime.Now.AddDays(-2), 
	            CaseId = 2 
	        });  
	    }
	}
}
```

>[!info] HasData  
>The `HasData` method is used to return data when `DbContext` is used.

## Repositories

Modify repositories to use the `DbContext`.

```C#
public class CaseRepository : ICaseRepository  
{  
    private readonly PocGraphQLContext _dbContext;  
  
    public CaseRepository(PocGraphQLContext dbContext)  
    {        
	    _dbContext = dbContext;  
    }    
    
    public IEnumerable<Case> GetAll() => _dbContext.Cases.ToList();  
}
```

```C#
public class ContractRepository : IContractRepository  
{  
    private readonly PocGraphQLContext _context;  
  
    public ContractRepository(PocGraphQLContext context)  
    {        _context = context;  
    }  
    public IEnumerable<Contract> GetByCase(int caseId) =>  
        _context.Contracts.Where(contract => contract.CaseId == caseId);  
  
    public async Task<ILookup<int, Contract>> GetContractsByCaseIdBatchAsync(IEnumerable<int> caseIds) =>  
        (await _context.Contracts.Where(contract => caseIds.Contains(contract.CaseId))  
            .ToListAsync())  
        .ToLookup(contract => contract.Id);  
}
```

Then modify the `program.cs` file to register the new implementation.

```C#
builder.Services.AddScoped<IContractRepository, ContractRepository>();  
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
```

# Use Sql Server

Install new package from nuget.

```cmd
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Add the connection string in your `appsettings.json` file :

```json
"ConnectionStrings": {  
  "Poc": "Server=localhost;Database=Poc;Trusted Connection=True;"
}
```

Then replace the `DbContext` registration in the `program.cs` file by the code below.

```c#
builder.Services.AddDbContext<PocGraphQLContext>(options =>   
    options.UseSqlServer(  
        builder.Configuration.GetConnectionString("Poc")));
```

---

Tags : #GraphQL, #EFCore, #AspNetCore,
