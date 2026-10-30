using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.Extensions.Configuration;
using WestCoastEducation.Core.Interfaces;

namespace WestCoastEducation.Core.Repositories;

public class HttpService : IStorageProvider, IDisposable
{
    private HttpClient _client { get; } = new();
    private readonly string _baseUrl = string.Empty;
    private readonly string _apiKey = string.Empty;
    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
        //Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    

    public HttpService()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("AppSettings.json")
            .Build();
        _baseUrl = config.GetSection("Settings:BASE_URL").Value!;
        _apiKey = config.GetSection("Settings:API_KEY").Value!;
        _client.BaseAddress = new Uri(_baseUrl);
        _client.DefaultRequestHeaders.Add("cache-Control", "no-cache");
        
        _client.DefaultRequestHeaders.Add("x-apikey", _apiKey);
    }
    
    public async Task Post<T>(T obj) where T : IStorageObject
    {
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        string url = $"{_baseUrl}/{obj.GetType().Name.ToLower()}";
        var responseMessage = await _client.PostAsJsonAsync(url, obj);
        Console.WriteLine(await responseMessage.Content.ReadAsStringAsync());
        Console.WriteLine(url);
        Console.WriteLine(responseMessage.StatusCode);
        

    }

    public async Task<T?> Get<T>(string id)
    {
        string url = $"{_baseUrl}/{typeof(T).Name.ToLower().Trim()}/{id}";
        return await _client.GetFromJsonAsync<T>(url);
    }

    public async Task Delete<T>(string id)
    {
        string url = $"{_baseUrl}/{typeof(T).Name.ToLower()}/{id}";
        await _client.DeleteAsync(url);
    }

    public async Task<List<T>> GetAll<T>()
    {
        string url = $"{_baseUrl}/{typeof(T).Name.ToLower()}";
        return await _client.GetFromJsonAsync<List<T>>(url) ?? [];
    }

    public async Task Put<T>(T obj) where T : IStorageObject
    {
        string url = $"{_baseUrl}/{obj.GetType().Name.ToLower()}/{obj.Id}";
        string jsonString = JsonSerializer.Serialize(obj);
        await _client.PutAsJsonAsync(url, jsonString);
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}