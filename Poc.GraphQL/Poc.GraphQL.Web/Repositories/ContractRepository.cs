using Poc.GraphQL.Web.Entities;
using Poc.GraphQL.Web.Repositories.Abstractions;
using Poc.GraphQL.Web.Repositories.EF;

namespace Poc.GraphQL.Web.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly PocGraphQLContext _context;

    public ContractRepository(PocGraphQLContext context)
    {
        _context = context;
    }

    public IEnumerable<Contract> GetByCase(int caseId) =>
        _context.Contracts.Where(reason => reason.CaseId == caseId);
}