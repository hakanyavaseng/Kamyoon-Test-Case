using ProductManagement.Domain.Entities.Common;

namespace ProductManagement.Domain.Entities;

public class Product : FullAuditedEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}