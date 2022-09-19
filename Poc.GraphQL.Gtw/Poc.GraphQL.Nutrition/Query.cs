using Poc.GraphQL.Nutrition.Entities;

namespace Poc.GraphQL.Nutrition;

public class Query
{
    private readonly IEnumerable<Food> _food = new List<Food>()
    {
        new() { Id = 1, Name = "Carotte"},
        new() { Id = 2, Name = "Oignon"},
        new() { Id = 3, Name = "Celeri"}
    };

    public IEnumerable<Food> AllFood()
        => _food;

    public Food FoodById(int id)
        => _food.Single(food => food.Id == id);
}