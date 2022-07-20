using GraphQL.Types;

namespace Poc.GraphQL.Web.GraphQL;

public class PocApplicationSchema : Schema
{
    public PocApplicationSchema(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<PocApplicationQuery>();
    }
}