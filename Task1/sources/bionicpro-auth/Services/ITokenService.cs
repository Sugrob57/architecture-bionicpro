using BionicProAuth.Models;

namespace BionicProAuth.Services
{
	// Services/ITokenService.cs
	public interface ITokenService
	{
		Task<SessionData> ExchangeCodeAsync(string code, string codeVerifier, string redirectUri);

		Task<SessionData> RefreshAsync(SessionData session);
	}
}