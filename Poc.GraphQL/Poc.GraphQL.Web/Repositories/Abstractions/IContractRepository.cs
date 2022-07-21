using Poc.GraphQL.Web.Entities;

namespace Poc.GraphQL.Web.Repositories.Abstractions;

public interface IContractRepository
{
    IEnumerable<Contract> GetByCase(int caseId);
}