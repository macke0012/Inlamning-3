namespace WestCoastEducation.Core.Interfaces;

public interface IService<T>: IDisposable
{
    Task Create(T obj);
    Task Update(T obj);
    Task Delete(T obj);
    Task<List<T>> GetAll();
    Task<T> GetSingle(string id);
}
