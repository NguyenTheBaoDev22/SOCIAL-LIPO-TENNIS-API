using Core;
using Core.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Interfaces;

namespace Infrastructure.Persistences.Interceptors
{
    /// <summary>
    /// Gán metadata (CreatedBy, UpdatedBy, TenantId, TraceId, ...) cho thực thể có kế thừa Audit
    /// </summary>
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUser;

        public AuditSaveChangesInterceptor(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplyAudit(DbContext? context)
        {
            if (context == null) return;

            var now = DateTime.UtcNow;
            var userId = _currentUser.UserId;
            var traceId = _currentUser.TraceId;
            var ip = _currentUser.IpAddress;

            foreach (var entry in context.ChangeTracker.Entries<Audit>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.TraceId = traceId;
                    entry.Entity.IpAddress = ip;

                    SetTenantInfo(entry);
                }

                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userId;
                    entry.Entity.TraceId = traceId;
                    entry.Entity.IpAddress = ip;
                }

                else if (entry.State == EntityState.Deleted)
                {
                    // Soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.DeletedBy = userId;

                    entry.Entity.TraceId = traceId;
                    entry.Entity.IpAddress = ip;
                }
            }
        }

        /// <summary>
        /// Gán TenantId, MerchantId, MerchantBranchId nếu là entity đa tenant
        /// </summary>
        private void SetTenantInfo(EntityEntry<Audit> entry)
        {
            if (entry.Entity is TenantEntity tenant)
            {
                tenant.TenantId = _currentUser.TenantId ?? Guid.Empty;
                tenant.MerchantId = _currentUser.MerchantId ?? Guid.Empty;
                tenant.MerchantBranchId = _currentUser.MerchantBranchId ?? Guid.Empty;
            }
        }
    }
}
