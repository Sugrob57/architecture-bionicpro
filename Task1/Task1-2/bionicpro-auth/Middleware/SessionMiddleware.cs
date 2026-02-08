using BionicProAuth.Services;

namespace BionicProAuth.Middleware
{
	public class SessionMiddleware
	{
		private readonly RequestDelegate _next;

		public SessionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(
			HttpContext ctx,
			ISessionStore store,
			ITokenService tokens)
		{
			if (!ctx.Request.Cookies.TryGetValue("BIONICPRO_SESSION", out var sessionId))
			{
				ctx.Response.StatusCode = 401;
				return;
			}

			var session = await store.GetAsync(sessionId);
			if (session == null)
			{
				ctx.Response.StatusCode = 401;
				return;
			}

			if (session.AccessTokenExpiresAt <= DateTime.UtcNow)
			{
				session = await tokens.RefreshAsync(session);
			}

			// rotation
			var ssesionTtl = Environment.GetEnvironmentVariable("SESSION_TTL");
			var ttl = string.IsNullOrEmpty(ssesionTtl) ? TimeSpan.FromMinutes(60) : TimeSpan.Parse(ssesionTtl);

			var maxAgeStr = Environment.GetEnvironmentVariable("COOKIE_MAX_AGE");
			var maxAge = string.IsNullOrEmpty(ssesionTtl) ? TimeSpan.FromMinutes(60) : TimeSpan.Parse(maxAgeStr);

			var newSessionId = Guid.NewGuid().ToString("N");
			await store.DeleteAsync(sessionId);
			await store.StoreAsync(newSessionId, session, ttl);

			ctx.Response.Cookies.Append(
				"BIONICPRO_SESSION",
				newSessionId,
				new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.Strict,
					MaxAge = maxAge
				});

			ctx.Items["AccessToken"] = session.AccessToken;

			await _next(ctx);
		}
	}
}