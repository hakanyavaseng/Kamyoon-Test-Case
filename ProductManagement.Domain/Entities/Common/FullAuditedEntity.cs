namespace ProductManagement.Domain.Entities.Common;

public abstract class FullAuditedEntity<TPrimaryKey> : ModificationAuditedEntity<TPrimaryKey>
{
    public DateTime? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }
    public bool? IsDeleted { get; set; } = false;
}