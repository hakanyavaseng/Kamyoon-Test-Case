namespace ProductManagement.Core.DTOs.Product;

public record AddProductDto
{
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
}