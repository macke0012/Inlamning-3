using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using WestCoastEducation.Core.Interfaces;

namespace WestCoastEducation.Core.Repositories;

public class DataBaseClient : IStorageProvider
{
    private readonly IMongoDatabase _database;

    static DataBaseClient()
    {
        // Runs once: camelCase field names, and ignore fields the class doesn't have
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true)
        };
        ConventionRegistry.Register("WestCoastConventions", pack, t => true);
    }

    public DataBaseClient()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionUri = config.GetSection("Settings:MONGODB_URI").Value!;
        Console.WriteLine(connectionUri);
        _database = new MongoClient(connectionUri).GetDatabase("WestCoastEducation");
    }

    private IMongoCollection<T> Collection<T>() =>
        _database.GetCollection<T>(typeof(T).Name.ToLower());

    private static FilterDefinition<T> ById<T>(string id) =>
        Builders<T>.Filter.Eq("_id", id);

    public async Task Post<T>(T obj) where T : IStorageObject
    {
        await Collection<T>().InsertOneAsync(obj); //Objektet läggs in på T's kollektion. Hade kunnat göra en Staff klass och inkludera läraren i den och ha dem alla där. MongoDB håller koll på typen.
    }

    public async Task<T?> Get<T>(string id)
    {
        return await Collection<T>().Find(ById<T>(id)).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetAll<T>()
    {
        return await Collection<T>().Find(Builders<T>.Filter.Empty).ToListAsync();
    }

    public async Task Put<T>(T obj) where T : IStorageObject
    {
        Console.WriteLine(obj.Id);
        Console.WriteLine(typeof(T).Name.ToLower());
        var result = await Collection<T>().ReplaceOneAsync(ById<T>(obj.Id), obj);
        Console.WriteLine($"Matched: {result.MatchedCount}, Modified: {result.ModifiedCount}");
    }

    public async Task Delete<T>(string id)
    {
        await Collection<T>().DeleteOneAsync(ById<T>(id));
    }

    public async Task<IChangeStreamCursor<ChangeStreamDocument<T>>> GetWatcher<T>()
    {
        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
        };
        return await Collection<T>().WatchAsync(options);
    }
}