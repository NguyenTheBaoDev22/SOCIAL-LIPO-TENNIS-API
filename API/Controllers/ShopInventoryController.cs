using Applications.Features.Shops.Inventories.Commands;
using Applications.Features.Shops.Inventories.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/shop-inventory")]
    public class ShopInventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShopInventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShopInventoryCommand command)
            => Ok(await _mediator.Send(command));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShopInventoryCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _mediator.Send(new DeleteShopInventoryAuditCommand { Id = id }));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
            => Ok(await _mediator.Send(new GetShopInventoryAuditByIdQuery { Id = id }));

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetShopInventoryAuditsQuery query)
            => Ok(await _mediator.Send(query));
    }

}
