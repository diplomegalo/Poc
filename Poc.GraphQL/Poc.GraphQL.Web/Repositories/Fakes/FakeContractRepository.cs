using Poc.GraphQL.Web.Entities;
using Poc.GraphQL.Web.Repositories.Abstractions;

namespace Poc.GraphQL.Web.Repositories.Fakes;

public class FakeContractRepository : IContractRepository
{
    public IEnumerable<Contract> GetByCase(int caseId)
    {
        return new List<Contract>()
        {
            { new () { Id = 1, Ref = "Ref1", CreationDate = DateTime.Now } },
            { new () { Id = 2, Ref = "Ref2", CreationDate = DateTime.Now } }
        };
    }
}