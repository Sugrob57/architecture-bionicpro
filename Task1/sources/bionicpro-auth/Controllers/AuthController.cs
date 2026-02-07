using BionicProAuth.Services;
using Microsoft.AspNetCore.Mvc;

namespace BionicProAuth.Controllers
{
	// Controllers/AuthController.cs
	[ApiController]
	[Route("auth")]
	public class AuthController : ControllerBase
	{
		private readonly ITokenService _tokens;
		private readonly ISessionStore _sessions;

		public AuthController(ITokenService tokens, ISessionStore sessions)
		{
			_tokens = tokens;
			_sessions = sessions;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest req)
		{
			var session = await _tokens.ExchangeCodeAsync(
				req.Code,
				req.CodeVerifier,
				req.RedirectUri);

			var sessionId = Guid.NewGuid().ToString("N");

			var ssesionTtl = Environment.GetEnvironmentVariable("SESSION_TTL");
			var ttl = string.IsNullOrEmpty(ssesionTtl) ? TimeSpan.FromMinutes(60) : TimeSpan.Parse(ssesionTtl);

			var maxAgeStr = Environment.GetEnvironmentVariable("COOKIE_MAX_AGE");
			var maxAge = string.IsNullOrEmpty(ssesionTtl) ? TimeSpan.FromMinutes(60) : TimeSpan.Parse(maxAgeStr);

			await _sessions.StoreAsync(sessionId, session, ttl);

			Response.Cookies.Append("BIONICPRO_SESSION", sessionId, CookieOptions(maxAge));

			return Ok();
		}

		private static CookieOptions CookieOptions(TimeSpan maxAge) => new()
		{
			HttpOnly = true,
			Secure = true,
			SameSite = SameSiteMode.Strict,
			MaxAge = maxAge
		};
	}

	public record LoginRequest(string Code, string CodeVerifier, string RedirectUri);
}