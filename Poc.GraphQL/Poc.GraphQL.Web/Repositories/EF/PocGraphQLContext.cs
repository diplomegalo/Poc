using Microsoft.EntityFrameworkCore;
using Poc.GraphQL.Web.Entities;

namespace Poc.GraphQL.Web.Repositories.EF;

public class PocGraphQLContext : DbContext
{
    public PocGraphQLContext(DbContextOptions<PocGraphQLContext> options)
        :base(options)
    {
    }
    
    public DbSet<Case> Cases { get; set; }
    public DbSet<Contract> Contracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Contract>().HasData(
            new Contract { Id = 1, Ref = "Ref-1", CreationDate = DateTime.Now.AddDays(-1), CaseId = 1 },
            new Contract { Id = 2, Ref = "Ref-2", CreationDate = DateTime.Now.AddDays(-2), CaseId = 2 });
    }
}