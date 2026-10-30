using Microsoft.Extensions.DependencyInjection;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Repositories;
using WestCoastEducation.Core.Services;

namespace WestCoastEducation.Core;

public static class ServiceCollectionExtensions
{

    public static void AddServices(this IServiceCollection collection)
    {
        collection.AddSingleton<IStorageProvider, DataBaseClient>();
        collection.AddSingleton<CourseService>();
        collection.AddSingleton<StudentService>();
        collection.AddSingleton<StaffService>();
        collection.AddSingleton<School>();
    }
}