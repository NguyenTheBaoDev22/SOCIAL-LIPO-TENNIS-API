using Applications.Features.ClientCredentials.Commands;
using Applications.Features.ClientCredentials.Queries;
using Core.Enumerables;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleEnum.AdminDashboard)]
    public class ClientCredentialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientCredentialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientCredentialCommand command) => Ok(await _mediator.Send(command));

        [HttpPut]
        public async Task<IActionResult> Update(UpdateClientCredentialCommand command) => Ok(await _mediator.Send(command));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _mediator.Send(new DeleteClientCredentialCommand { Id = id }));

        [HttpPost("{id}/reset-secret")]
        public async Task<IActionResult> ResetSecret(Guid id) => Ok(await _mediator.Send(new ResetClientSecretCommand { Id = id }));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _mediator.Send(new GetClientCredentialByIdQuery { Id = id }));

        [HttpGet]
        public async Task<IActionResult> GetList() => Ok(await _mediator.Send(new GetClientCredentialListQuery()));

    }
}
