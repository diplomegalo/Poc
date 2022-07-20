using GraphQL.Types;
using Poc.GraphQL.Web.Entities;
using Poc.GraphQL.Web.GraphQL.Types;

namespace Poc.GraphQL.Web.GraphQL;

public class PocApplicationQuery : ObjectGraphType
{
    public PocApplicationQuery()
    {
        Field<ListGraphType<CaseType>>(
            "dossier",
            resolve: _ => new List<Case>()
            {
                new() { Id = 1, Ref = "plop reference 1" },
                new() { Id = 2, Ref = "plop reference 2", Status = CaseStatus.InProgress },
            });
    }
}