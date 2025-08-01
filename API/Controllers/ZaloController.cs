using Applications.Features.FileUpload.Commands;
using Applications.Features.ZaloIntegrations.ZaloAuth.Commands;
using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using Applications.Features.ZaloIntegrations.ZaloAuth.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZaloController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ZaloController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Lấy số điện thoại người dùng từ AccessToken và PhoneTokenCode của Zalo.
        /// </summary>
        /// <param name="command">Dữ liệu gồm AccessToken và PhoneTokenCode</param>
        /// <returns>Thông tin người dùng Zalo gồm số điện thoại</returns>
        [HttpPost("auth/get-phone-number")]
        public async Task<ActionResult<BaseResponse<UserZaloIdentityResponseModel>>> GetPhoneNumberAsync(
            [FromBody] GetPhoneNumberFromTokenCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            var data = new UserZaloIdentityResponseModel { UserPhoneNumber = "0937127023" };
            result.Data = data;
            return Ok(result);
        }
        [HttpPost("upload-media")]
        public async Task<IActionResult> UploadFromZalo([FromForm] IFormFileCollection file)
        {
            var result = await _mediator.Send(new UploadZaloMediaCommand { Files = file.ToList() });

            return StatusCode(Convert.ToInt32(result.Code) == 0 ? 200 : 400, new
            {
                error = result.Code,
                message = result.Message,
                data = result.Data == null ? null : new { result.Data.Urls }
            });
        }
        /// <summary>
        /// Lấy vị trí người dùng Zalo từ AccessToken và LocationToken (code).
        /// </summary>
        /// <param name="query">Dữ liệu gồm AccessToken và LocationToken</param>
        /// <returns>Thông tin vị trí (lat, long, provider...)</returns>
        [HttpPost("get-location")]
        public async Task<ActionResult<BaseResponse<LocationResultDto>>> GetLocationAsync(
            [FromBody] GetZaloLocationQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok( result);
            //return StatusCode(Convert.ToInt32(result.Code) == 0 ? 200 : 400, new
            //{
            //    error = result.Code,
            //    message = result.Message,
            //    data = mockData result.Data == null ? null : new { result.Data }
            //});


        }
    }
}
