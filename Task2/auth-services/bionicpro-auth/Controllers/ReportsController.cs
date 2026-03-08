using BionicProAuth.Services;
using Microsoft.AspNetCore.Mvc;
using ISessionStore = BionicProAuth.Services.ISessionStore;

namespace BionicProAuth.Controllers
{
	// Controllers/AuthController.cs
	[ApiController]
	[Route("api/v1/reports")]
	public class ReportsController : ControllerBase
	{
		public ReportsController(
			ITokenService tokens,
			ISessionStore sessionStore,
			IConfiguration cfg,
			ReportService reportService)
		{
			_tokens = tokens;
			_cfg = cfg;
			_sessionStore = sessionStore;
			_reportService = reportService;
		}

		[HttpGet("me")]
		public IActionResult GetReports()
		{
			Console.WriteLine("me requested");
			// если мы тут — значит SessionMiddleware пропустил
			return Ok(new { authenticated = true });
		}

		[HttpGet("telemetry")]
		public async Task<IActionResult> GetTelemetryReportAsync()
		{
			string sessionId = null;

			if (HttpContext.Request.Cookies.TryGetValue("BIONICPRO_SESSION", out sessionId))
			{
				return Unauthorized("BIONICPRO_SESSION cookie not found");
			}

			var session = await _sessionStore.GetAsync(sessionId);

			if (session == null)
			{
				return NotFound($"Session for {sessionId} not stored");
			}

			string userId = session.UserEmail;

			if (userId == null)
			{
				return BadRequest($"User email is empty for {sessionId}");
			}

			var report = await _reportService.GetUserReportAsync(userId);

			return Ok(report);
		}

		[HttpGet("user")]
		public async Task<IActionResult> GetUserInfoAsync()
		{
			string sessionId = null;

			if (HttpContext.Request.Cookies.TryGetValue("BIONICPRO_SESSION", out sessionId))
			{
				return Unauthorized("BIONICPRO_SESSION cookie not found");
			}

			var session = await _sessionStore.GetAsync(sessionId);

			if (session == null)
			{
				return Unauthorized($"Session for {sessionId} not stored");
			}

			return Ok(session);
		}

		

		private readonly IConfiguration _cfg;
		private readonly ISessionStore _sessionStore;
		private readonly ITokenService _tokens;
		private readonly ReportService _reportService;
	}
}