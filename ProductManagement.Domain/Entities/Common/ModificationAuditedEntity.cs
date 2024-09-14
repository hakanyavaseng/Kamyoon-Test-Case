namespace ProductManagement.Domain.Entities.Common;

public abstract class ModificationAuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>
{
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
}