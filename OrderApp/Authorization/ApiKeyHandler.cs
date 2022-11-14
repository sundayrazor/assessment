using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace OrderApp.Authorization;

public class ApiKeyHandler
{
	private const string ApiKey = "X-API-KEY";
	private const string ApiKeyValue = "5E5B8267";
	private readonly Uri _baseAddress = new(" https://fcinterviewapi01.azurewebsites.net/api/");
	private readonly HttpClient _client;

	public ApiKeyHandler()
	{
		_client = new();
		_client.BaseAddress = _baseAddress;

		_client.DefaultRequestHeaders.Accept.Clear();
		_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_client.DefaultRequestHeaders.Add(ApiKey, ApiKeyValue);
	}

	public HttpResponseMessage SendGetRequest(string req) =>
		_client.GetAsync(_client.BaseAddress + req).Result;


	public HttpResponseMessage SendPostRequest(object obj, string req)
	{
		var data = JsonConvert.SerializeObject(obj);
		StringContent content = new(data, Encoding.UTF8, "application/json");
		return _client.PostAsync(_client.BaseAddress + req, content).Result;

	}
}