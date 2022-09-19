using Poc.GraphQL.Nutrition.Entities;

namespace Poc.GraphQL.Nutrition;

public class Query
{
    private readonly IEnumerable<Food> _food = new List<Food>()
    {
        new(1, "Carotte"),
        new(2, "Oignon"),
        new(3, "Celeri")
    };

    public IEnumerable<Food> Foods()
        => _food;

    public Food Food(int id)
        => _food.Single(food => food.Id == id);
}