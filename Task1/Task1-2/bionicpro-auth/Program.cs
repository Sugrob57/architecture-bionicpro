using BionicProAuth.Middleware;
using BionicProAuth.Services;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<ITokenService, KeycloakTokenService>();

var redisConnectionString = builder.Configuration["Redis"]!;
var hostingPort = builder.Configuration["Hosting.Port"]!;


builder.Services.AddSingleton<IConnectionMultiplexer>(
	ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddDataProtection()
	.PersistKeysToStackExchangeRedis(
		ConnectionMultiplexer.Connect(redisConnectionString), "dp-keys")
	.SetApplicationName("bionicpro-auth");

builder.Services.AddScoped<ISessionStore, RedisSessionStore>();

var app = builder.Build();

app.UseMiddleware<SessionMiddleware>();
app.MapControllers();

app.Run($"http://0.0.0.0:{hostingPort}");