using Applications.Features.ClientCredentials.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context; // 👈 Quan trọng: dùng để push LogContext
using static API.Extensions.HttpResponseExtensions;
namespace ZenShopAPI.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Đăng nhập bằng Client Credential để nhận JWT Token.
        /// </summary>
        [HttpPost("apg/oauth2/token")]
        public async Task<IActionResult> GetToken([FromBody] ClientCredentialLoginCommand command)
        {
            var traceId = HttpContext.TraceIdentifier;

            using (LogContext.PushProperty("TraceId", traceId))
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("❌ Đăng nhập thất bại. ClientId: {ClientId}", command.ClientId);
                    return StatusCode(MapHttpStatusCode(result.Code), result);
                }

                _logger.LogInformation("✅ Đăng nhập thành công. ClientId: {ClientId}", command.ClientId);
                return StatusCode(200, result); // hoặc return Ok(result);
            }
        }

    }
}
