namespace ProductManagement.Core.DTOs.Product;

public record ListProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? LastModifiedDate { get; init; }
}