using BionicProAuth.Models;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using System.Text.Json;

namespace BionicProAuth.Services
{
	public class RedisSessionStore : ISessionStore
	{
		private readonly IDatabase _db;
		private readonly IDataProtector _protector;

		public RedisSessionStore(IConnectionMultiplexer redis, IDataProtectionProvider dp)
		{
			_db = redis.GetDatabase();
			_protector = dp.CreateProtector("bionicpro-auth.tokens");
		}

		public async Task StoreAsync(string sessionId, SessionData session, TimeSpan ttl)
		{
			var json = JsonSerializer.Serialize(session);
			var encrypted = _protector.Protect(json);

			await _db.StringSetAsync(Key(sessionId), encrypted, ttl);
		}

		public async Task<SessionData?> GetAsync(string sessionId)
		{
			var encrypted = await _db.StringGetAsync(Key(sessionId));
			if (encrypted.IsNullOrEmpty) return null;

			var json = _protector.Unprotect(encrypted!);
			return JsonSerializer.Deserialize<SessionData>(json);
		}

		public Task DeleteAsync(string sessionId)
			=> _db.KeyDeleteAsync(Key(sessionId));

		private static string Key(string id) => $"session:{id}";
	}
}