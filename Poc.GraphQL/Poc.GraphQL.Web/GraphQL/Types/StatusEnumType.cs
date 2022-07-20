using GraphQL.Types;
using Poc.GraphQL.Web.Entities;

namespace Poc.GraphQL.Web.GraphQL.Types;

public class StatusEnumType : EnumerationGraphType<CaseStatus>
{
    public StatusEnumType()
    {
        Name = "Status";
        Description = "The case status.";
    }
}