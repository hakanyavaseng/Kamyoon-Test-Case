using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Entities.Common;

namespace ProductManagement.Persistence.Contexts;

public class AppDbContext : IdentityDbContext<AppUser,AppRole,Guid>
{
    public DbSet<Product> Products { get; set; }
    
    
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    //TODO : Current user id should be taken from http context
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && 
                        (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

        foreach (var entry in entries)
        {
            if (entry.Entity is CreationAuditedEntity<Guid> creationAuditedEntity && entry.State == EntityState.Added)
            {
                creationAuditedEntity.Id = Guid.NewGuid();
                creationAuditedEntity.CreatedDate = DateTime.UtcNow;
                creationAuditedEntity.CreatedBy = Guid.NewGuid(); 
            }

            if (entry.Entity is ModificationAuditedEntity<Guid> modificationAuditedEntity && entry.State == EntityState.Modified)
            {
                modificationAuditedEntity.LastModifiedDate = DateTime.UtcNow;
                modificationAuditedEntity.LastModifiedBy = "system"; 
            }

            if (entry.Entity is FullAuditedEntity<Guid> fullAuditedEntity && entry.State == EntityState.Deleted)
            {
                fullAuditedEntity.DeletedDate = DateTime.UtcNow;
                fullAuditedEntity.DeletedBy = "system"; 
                fullAuditedEntity.IsDeleted = true;

                entry.State = EntityState.Modified;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}