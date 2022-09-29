using Poc.GraphQL.Common;

namespace Poc.GraphQL.Nutrition.Entities;
public record Food(int Id, string Name, NutritionalValue? NutritionalValue = null);