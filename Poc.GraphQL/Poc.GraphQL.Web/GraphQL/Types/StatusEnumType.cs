using GraphQL.Types;
using Poc.GraphQL.Web.Entities;

namespace Poc.GraphQL.Web.GraphQL.Types;

public class StatusEnumType : EnumerationGraphType<Status>
{
    public StatusEnumType()
    {
        Name = "Status";
        Description = "The status of the case.";
    }
}