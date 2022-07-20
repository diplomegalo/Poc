using GraphQL.Types;
using Poc.GraphQL.Web.Entities;

namespace Poc.GraphQL.Web.GraphQL.Types;

public sealed class CaseType : ObjectGraphType<Case>
{
    public CaseType()
    {
        Name = "Case";
        Description = "A case contains information about the customer.";
        Field(dossier => dossier.Id).Description("The case unique identifier.");
        Field(dossier => dossier.Ref).Description("The case business reference.");
        
        // Enum type
        Field<StatusEnumType>("Status", "The status of the case.");
    }
}