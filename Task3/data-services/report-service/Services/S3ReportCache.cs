using Amazon.S3;
using Amazon.S3.Model;
using System.Text.Json;
using ReportService.Models;

namespace ReportService.Services;

public class S3ReportCache
{
	private readonly IAmazonS3 _s3;
	private readonly string _bucket;

	public S3ReportCache(IConfiguration config)
	{
		var cfg = new AmazonS3Config
		{
			ServiceURL = config["S3:ServiceUrl"],
			ForcePathStyle = true
		};

		_s3 = new AmazonS3Client(
			config["S3:AccessKey"],
			config["S3:SecretKey"],
			cfg);

		_bucket = config["S3:Bucket"];
	}

	public async Task<ClientTelemetryReport?> GetAsync(string clientId)
	{
		var key = $"reports/{clientId}.json";

		try
		{
			var response = await _s3.GetObjectAsync(_bucket, key);

			using var reader = new StreamReader(response.ResponseStream);
			var json = await reader.ReadToEndAsync();

			return JsonSerializer.Deserialize<ClientTelemetryReport>(json);
		}
		catch (AmazonS3Exception e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return null;
		}
	}

	public async Task SaveAsync(string clientId, ClientTelemetryReport report)
	{
		var key = $"reports/{clientId}.json";

		var json = JsonSerializer.Serialize(report);

		var request = new PutObjectRequest
		{
			BucketName = _bucket,
			Key = key,
			ContentBody = json,
			ContentType = "application/json"
		};

		await _s3.PutObjectAsync(request);
	}
}