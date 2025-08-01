using Applications.Features.ClientCredentials.Commands;
using Applications.Features.LarksuiteIntegrations.DTOs;
using Applications.Services.Implementations;
using Applications.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICacheService _cache;
        private readonly ILarkTokenService _larkTokenService;
        public TokenController(IMediator mediator, ICacheService cache, ILarkTokenService larkTokenService)
        {
            _mediator = mediator; 
            _cache = cache;
            _larkTokenService = larkTokenService;
        }
    
        [HttpPost("client-credential")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromBody] ClientCredentialLoginCommand command)
            => Ok(await _mediator.Send(command));
        [HttpGet("larksuite-access-token")]
        public IActionResult GetLarkTokenCache()
        {
            var token = _cache.Get<LarkTokenCache>(CacheKeys.LarkToken);
            if (token == null)
                return NotFound("Cache miss hoặc hết hạn.");
            return Ok(token);
        }
        [HttpPost("larksuite-refresh-token")]
        public async Task<IActionResult> RefreshLarkToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest("RefreshToken is required.");

            var refreshedToken = await _larkTokenService.RefreshAccessTokenAsync(request.RefreshToken);

            if (refreshedToken == null)
                return BadRequest("Refresh token failed or expired.");

            // Optionally return token info or just new refresh token
            return Ok(new
            {
                AccessToken = refreshedToken.AccessToken,
                RefreshToken = refreshedToken.RefreshToken,
                ExpiresAt = refreshedToken.AccessTokenExpiresAt,
                RefreshTokenExpiresAt = refreshedToken.RefreshTokenExpiresAt
            });
        }
    }
}
