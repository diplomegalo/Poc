using Poc.GraphQL.Common;

namespace Poc.GraphQL.Recipes.Entities;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
    public NutritionalValue NutritionalValue { get; set; }
}