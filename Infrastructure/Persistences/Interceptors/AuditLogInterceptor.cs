using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Interfaces;
using System.Text.Json;

namespace Infrastructure.Persistences.Interceptors
{
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUser;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuditLogInterceptor(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            AddAuditLogs(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            AddAuditLogs(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void AddAuditLogs(DbContext? context)
        {
            if (context == null) return;

            var now = DateTime.UtcNow;
            var logs = new List<AuditLog>();

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged &&
                            e.State != EntityState.Detached &&
                            e.Entity is not AuditLog);

            foreach (var entry in entries)
            {
                var entityName = entry.Entity.GetType().Name;
                var entityId = GetPrimaryKey(entry);
                var changes = GetChanges(entry);

                if (changes == null) continue; // không có thay đổi thực sự

                logs.Add(new AuditLog
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    ActionType = entry.State.ToString(),
                    ChangedAt = now,
                    ChangedBy = _currentUser.UserId?.ToString(),
                    TraceId = _currentUser.TraceId,
                    TenantId = _currentUser.TenantId,
                    Changes = JsonSerializer.Serialize(changes, _jsonOptions)
                });
            }

            if (logs.Count > 0)
                context.Set<AuditLog>().AddRange(logs);
        }

        private object? GetChanges(EntityEntry entry)
        {
            return entry.State switch
            {
                EntityState.Added => entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue),

                EntityState.Deleted => entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue),

                EntityState.Modified => GetModifiedProperties(entry),

                _ => null
            };
        }

        private Dictionary<string, object>? GetModifiedProperties(EntityEntry entry)
        {
            var dict = new Dictionary<string, object>();

            foreach (var prop in entry.Properties)
            {
                var original = prop.OriginalValue;
                var current = prop.CurrentValue;

                if (!Equals(original, current))
                {
                    dict[prop.Metadata.Name] = new
                    {
                        Old = original,
                        New = current
                    };
                }
            }

            return dict.Count > 0 ? dict : null;
        }

        private string GetPrimaryKey(EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key == null) return string.Empty;

            return string.Join(",", key.Properties
                .Select(p => entry.Property(p.Name).CurrentValue?.ToString() ?? string.Empty));
        }
    }
}
