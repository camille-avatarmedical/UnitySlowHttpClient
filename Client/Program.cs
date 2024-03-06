// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
public static class Program
{
	public static async Task Main()
	{
		var lines = System.IO.File.ReadAllLines("../serverUrl.txt");

		Console.WriteLine("HTTPS");
		await DoHttpRequest(lines[0]);

		Console.WriteLine("HTTP");
		await DoHttpRequest(lines[1]);
	}

	private static async Task DoHttpRequest(string url)
	{
		var stopwatch = new Stopwatch();
		stopwatch.Start();
		var httpClient2 = new HttpClient();
		var response2 = await httpClient2.GetAsync(url);
		var byteArray2 = await response2.Content.ReadAsByteArrayAsync();
		Console.WriteLine($"download time using HttpClient {stopwatch.ElapsedMilliseconds} ms");
		Console.WriteLine($"SHA1 using HttpClient {GetHashSHA1(byteArray2)}");
	}

	public static string GetHashSHA1(byte[] data)
	{
		using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
		{
			return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
		}
	}
}