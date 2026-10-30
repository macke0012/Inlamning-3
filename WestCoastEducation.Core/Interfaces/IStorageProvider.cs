namespace WestCoastEducation.Core.Interfaces;

public interface IStorageProvider
{
    Task Post<T>(T obj) where T : IStorageObject;
    Task<T?> Get<T>(string id);
    Task Delete<T>(string id);
    Task<List<T>> GetAll<T>();
    Task Put<T>(T obj) where T : IStorageObject;
}
