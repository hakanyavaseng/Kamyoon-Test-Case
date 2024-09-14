namespace ProductManagement.Core.DTOs.Product;

public record UpdateProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
}