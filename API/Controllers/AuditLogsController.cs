using Core.Entities;
using Infrastructure.Persistences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Authorization;
using Shared.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [AuthorizePermission(PermissionCode.ViewAuditLog)]
    public class AuditLogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuditLogsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<BaseResponse<PaginatedResult<AuditLog>>> GetAuditLogs(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? userId = null,
        [FromQuery] string? entityName = null,
        [FromQuery] string? actionType = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] string? sortField = "ChangedAt",
        [FromQuery] string? sortDirection = "desc")
        {
            var query = _context.AuditLogs.AsQueryable();

            // Filter theo TenantId
            var tenantId = HttpContext.User.FindFirst("TenantId")?.Value;
            if (!string.IsNullOrEmpty(tenantId) && Guid.TryParse(tenantId, out var parsedTenant))
            {
                query = query.Where(x => x.TenantId == parsedTenant);
            }

            if (userId.HasValue)
                query = query.Where(x => x.ChangedBy == userId.ToString());

            if (!string.IsNullOrEmpty(entityName))
                query = query.Where(x => x.EntityName == entityName);

            if (!string.IsNullOrEmpty(actionType))
                query = query.Where(x => x.ActionType == actionType);

            if (from.HasValue)
                query = query.Where(x => x.ChangedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(x => x.ChangedAt <= to.Value);

            return BaseResponse<AuditLog>.SuccessPaginated(
                query, pageIndex, pageSize, sortField: sortField, sortDirection: sortDirection);
        }
    }
}
