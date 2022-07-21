using GraphQL.Types;
using Poc.GraphQL.Web.Entities;
using Poc.GraphQL.Web.Repositories.Abstractions;

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

        // Contracts
        Field<ListGraphType<ContractType>>("contracts",
            resolve: context =>
            {
                var contactRepository = context.RequestServices?.GetRequiredService<IContractRepository>() ??
                                 throw new InvalidOperationException("Unable to retrieve service provider.");
                return contactRepository.GetByCase(context.Source.Id);
            });
    }
}