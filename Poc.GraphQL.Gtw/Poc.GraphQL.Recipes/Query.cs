using Poc.GraphQL.Recipes.Entities;

namespace Poc.GraphQL.Recipes;

public class Query
{
    private IEnumerable<Recipe> _recipes = new List<Recipe>()
    {
        new Recipe()
        {
            Id = 1, Name = "Sofritto", Ingredients = new List<Ingredient>()
            {
                new Ingredient() { FoodId = 1, Id = 1, Quantity = 1 },
                new Ingredient() { FoodId = 2, Id = 2, Quantity = 2 },
                new Ingredient() { FoodId = 3, Id = 3, Quantity = 3 }
            }
        }
    };

    public IEnumerable<Recipe> GetRecipes()
        => _recipes;
}