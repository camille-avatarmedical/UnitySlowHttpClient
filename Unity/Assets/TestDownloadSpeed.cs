using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TestDownloadSpeed : MonoBehaviour
{
	static string[] GetRequestUrls()
	{
		return System.IO.File.ReadAllLines(UnityEngine.Application.dataPath + "/../../serverUrl.txt");
	}

	async void Start()
    {
		var urls = GetRequestUrls();
		try
		{
			Debug.LogError("HTTPS");
			await DoWebRequests(urls[0]);

			Debug.LogError("HTTP");
			await DoWebRequests(urls[1]);

			StartCoroutine(DoWebRequests(urls));
		}
		catch (System.Exception e)
		{
			Debug.LogError(e);
		}
	}

	private async Task DoWebRequests(string url)
	{
		var httpClient = new HttpClient();
		var stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		var response = await httpClient.GetAsync(url);
		var byteArray = await response.Content.ReadAsByteArrayAsync();
		Debug.LogError($"Download time using HttpClient {stopwatch.ElapsedMilliseconds} ms");
		Debug.LogError($"SHA1 using HttpClient {GetHashSHA1(byteArray)}");
	}

	IEnumerator DoWebRequests(string[] urls)
	{
		Debug.LogError("HTTPS");
		yield return DoWebRequest(urls[0]);

		Debug.LogError("HTTP");
		yield return DoWebRequest(urls[1]);
	}

	IEnumerator DoWebRequest(string url)
	{
		var stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		var request2 = UnityWebRequest.Get(url);
		var operation2 = request2.SendWebRequest();
		while (!operation2.isDone || operation2.webRequest.result == UnityWebRequest.Result.InProgress)
		{
			yield return null;
		}
		var bytes = operation2.webRequest.downloadHandler.data;
		Debug.LogError($"Download time using UnityWebRequest {stopwatch.ElapsedMilliseconds} ms");
		Debug.LogError($"SHA1 using UnityWebRequest {GetHashSHA1(bytes)}");
	}

	public static string GetHashSHA1(byte[] data)
	{
		using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
		{
			return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
		}
	}
}
