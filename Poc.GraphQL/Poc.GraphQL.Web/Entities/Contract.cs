namespace Poc.GraphQL.Web.Entities;

public class Contract
{
    public int Id { get; set; }
    public string Ref { get; set; }
    public DateTime CreationDate { get; set; }
    
    // Case
    public int CaseId { get; set; }
    public Case Case { get; set; }
}