using WestCoastEducation.Core.Interfaces;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace WestCoastEducation.Core.Repositories;



public class JsonStorageService : IStorageProvider
{


    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };


    public Task<T?> Get<T>(string id)
    {
        try
        {
            string path = GetFilePath<T>(id);
            string jsonString = File.ReadAllText(path);

            return Task.FromResult(JsonSerializer.Deserialize<T>(jsonString, _options));
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            return default;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            return default;
        }
    }

    public Task Delete<T>(string id)
    {
        var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Data"), "*.json", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (Path.GetFileNameWithoutExtension(file) == id)
            {
                File.Delete(file);
            }
        }
        return Task.CompletedTask;
    }

    public Task<List<T>> GetAll<T>()
    {
        if (!Directory.Exists(GetDirectory<T>()))
        {
            return Task.FromResult(new List<T>());
        }

        string[] files = Directory.GetFiles(GetDirectory<T>(), "*.json");

        List<T> entries = [];

        foreach (var entry in files)
        {
            string content = File.ReadAllText(entry);
            T parsedEntry = JsonSerializer.Deserialize<T>(content, _options)!;
            entries.Add(parsedEntry!);
        }

        return Task.FromResult(entries);
    }

    public Task Put<T>(T obj) where T : IStorageObject
    {
        throw new NotImplementedException();
    }


    public Task Post<T>(T obj) where T : IStorageObject
    {
        var directory = Path.Combine(Environment.CurrentDirectory, "Data", obj.GetType().Name);

        Directory.CreateDirectory(directory);

        var jsonString = JsonSerializer.Serialize(obj, obj.GetType(), _options);

        var path = Path.Combine(directory, $"{obj.Id}.json");

        File.WriteAllText(path, jsonString.Trim());
        Console.WriteLine($"Saved {obj.GetType().Name} with ID {obj.Id} to {path}");
        return Task.CompletedTask;
    }
    private string GetFilePath<T>(string id) => $"{Environment.CurrentDirectory}/Data/{typeof(T).Name}/{id}.json";
    private string GetDirectory<T>() => $"{Environment.CurrentDirectory}/Data/{typeof(T).Name}";



}
