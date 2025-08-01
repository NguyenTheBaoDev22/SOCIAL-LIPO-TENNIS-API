using Applications.Features.Users.Commands;
using Applications.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Authorization;
using Shared.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission(PermissionCode.ManageUsers)]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gán vai trò cho người dùng trong phạm vi Merchant/Branch
        /// </summary>
        [HttpPost("assign-role")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleToUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Lấy danh sách quyền của người dùng theo Merchant/Branch
        /// </summary>
        [HttpGet("{userId}/permissions")]
        [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPermissions(
            Guid userId,
            [FromQuery] Guid merchantId,
            [FromQuery] Guid? merchantBranchId)
        {
            var query = new GetUserPermissionsQuery
            {
                UserId = userId,
                MerchantId = merchantId,
                MerchantBranchId = merchantBranchId
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command) => Ok(await _mediator.Send(command));

        [HttpPost("{id}/assign-roles")]
        public async Task<IActionResult> AssignRoles(Guid id, AssignRoleToUserCommand command)
        {
            command.UserId = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserCommand command)
        {
            command.UserId = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _mediator.Send(new GetUserByIdQuery(id)));

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetUserListQuery query) => Ok(await _mediator.Send(query));

        [HttpPut("{userId}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(Guid userId, [FromBody] bool isActive)
        {
            var command = new ToggleUserStatusCommand
            {
                UserId = userId,
                IsActive = isActive
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
