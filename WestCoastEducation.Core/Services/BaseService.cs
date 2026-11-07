using WestCoastEducation.Core.Interfaces;

namespace WestCoastEducation.Core.Services;

public abstract class BaseService<T>(IStorageProvider db) : IService<T>
    where T : IStorageObject
{
    public async Task Create(T obj) => await db.Post(obj);
    public async Task Update(T obj) => await db.Put(obj);
    public async Task Delete(T obj) => await db.Delete<T>(obj.Id);



    public async Task<List<T>> GetAll()
    {
        return await db.GetAll<T>();
    }

    public async Task<T> GetSingle(string id) => await db.Get<T>(id) ?? throw new KeyNotFoundException("No object with id: " + id + "");

    
}