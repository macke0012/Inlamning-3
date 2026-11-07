using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using WestCoastEducation.Core.Interfaces;

namespace WestCoastEducation.Core.Repositories;

public class HttpService : IStorageProvider, IDisposable
{
    private HttpClient Client { get; } = new();
    private readonly string _baseUrl;

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
        var apiKey = config.GetSection("Settings:API_KEY").Value!;
        Client.BaseAddress = new Uri(_baseUrl);
        Client.DefaultRequestHeaders.Add("cache-Control", "no-cache");
        
        Client.DefaultRequestHeaders.Add("x-apikey", apiKey);
    }
    
    public async Task Post<T>(T obj) where T : IStorageObject
    {
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        string url = $"{_baseUrl}/{obj.GetType().Name.ToLower()}";
        var responseMessage = await Client.PostAsJsonAsync(url, obj, _options);
        Console.WriteLine(await responseMessage.Content.ReadAsStringAsync());
        Console.WriteLine(url);
        Console.WriteLine(responseMessage.StatusCode);
        

    }

    public async Task<T?> Get<T>(string id)
    {
        string url = $"{_baseUrl}/{typeof(T).Name.ToLower().Trim()}/{id}";
        return await Client.GetFromJsonAsync<T>(url, _options);
    }

    public async Task Delete<T>(string id)
    {
        string url = $"{_baseUrl}/{typeof(T).Name.ToLower()}/{id}";
        await Client.DeleteAsync(url);
    }

    public async Task<List<T>> GetAll<T>()
    {
        string url = $"{_baseUrl}/{typeof(T).Name.ToLower()}";
        return await Client.GetFromJsonAsync<List<T>>(url, _options) ?? [];
    }

    public async Task Put<T>(T obj) where T : IStorageObject
    {
        string url = $"{_baseUrl}/{obj.GetType().Name.ToLower()}/{obj.Id}";
        string jsonString = JsonSerializer.Serialize(obj);
        await Client.PutAsJsonAsync(url, jsonString, _options);
    }

    public void Dispose()
    {
        Client.Dispose();
        GC.SuppressFinalize(this);
    }
}