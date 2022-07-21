namespace Poc.GraphQL.Web.Entities;

public class Case
{
    public int Id { get; set; }
    public string? Ref { get; set; }
    public Status Status { get; set; }
    
    public ICollection<Contract> Contracts { get; set; }
}