namespace ProductManagement.Domain.Entities.Common;

public abstract class CreationAuditedEntity<TPrimaryKey> : BaseEntity
{
    public TPrimaryKey Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public TPrimaryKey? CreatedBy { get; set; }
}