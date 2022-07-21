using GraphQL.Types;
using Poc.GraphQL.Web.Entities;

namespace Poc.GraphQL.Web.GraphQL.Types;

public sealed class ContractType : ObjectGraphType<Contract>
{
    public ContractType()
    {
        Field(contract => contract.Id);
        Field(contract => contract.Ref);
        Field(contract => contract.CreationDate);
    }
}