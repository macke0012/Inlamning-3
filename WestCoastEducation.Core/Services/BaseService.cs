using MongoDB.Driver;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Repositories;

namespace WestCoastEducation.Core.Services;

public abstract class BaseService<T> : IService<T> where T : IStorageObject
{
    protected IStorageProvider _db;
    protected IChangeStreamCursor<ChangeStreamDocument<T>>? _cursor;
    public BaseService(IStorageProvider db)
    {
        _db = db;
        if (_db.GetType() == typeof(DataBaseClient))
        {
            _cursor = Task.Run(() => (_db as DataBaseClient).GetWatcher<T>()).Result;
        }
    }
    public virtual void Dispose()
    {
        _cursor?.Dispose();
    }

    public async Task Create(T obj) => await _db.Post(obj);
    public async Task Update(T obj) => await _db.Put(obj);
    public async Task Delete(T obj) => await _db.Delete<T>(obj.Id);



    public async Task<List<T>> GetAll()
    {
        return await _db.GetAll<T>();
    }

    public async Task<T> GetSingle(string id) => await _db.Get<T>(id) ?? throw new KeyNotFoundException("No object with id: " + id + "");

    
}