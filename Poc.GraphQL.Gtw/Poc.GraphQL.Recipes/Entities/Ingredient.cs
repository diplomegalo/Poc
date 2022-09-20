namespace Poc.GraphQL.Recipes.Entities;

public class Ingredient
{
    public int Id { get; set; }
    public int FoodId { get; set; }
    public decimal Quantity { get; set; }
}