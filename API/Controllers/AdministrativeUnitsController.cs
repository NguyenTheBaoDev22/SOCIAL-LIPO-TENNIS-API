using Applications.Features.ImportData.Queries;
using Applications.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrativeUnitsController : ControllerBase
    {
        private readonly IAdministrativeDataService _dataService;
        private readonly IMediator _mediator;
        public AdministrativeUnitsController(IAdministrativeDataService dataService, IMediator mediator)
        {
            _dataService = dataService;
            _mediator = mediator;
        }
        [HttpGet("province-with-communes")]
        public async Task<IActionResult> GetAllAdministrativeUnits()
        {
            var result = await _mediator.Send(new GetAllAdministrativeUnitsQuery
            {
            });

            return Ok( result);
        }

        [HttpGet("province-with-communes/paginated")]
        public async Task<IActionResult> GetAllAdministrativeUnitsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllAdministrativeUnitsPaginatedQuery
            {
                Page = page,
                PageSize = pageSize
            });

            return Ok(result);
        }

    }
}
