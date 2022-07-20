namespace Poc.GraphQL.Web.Entities;

public class Case
{
    public int Id { get; set; }
    public string? Ref { get; set; }
    public CaseStatus Status { get; set; }
}