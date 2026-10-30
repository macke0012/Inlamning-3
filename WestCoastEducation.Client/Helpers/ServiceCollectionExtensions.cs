using System;
using Microsoft.Extensions.DependencyInjection;
using WestCoastEducation.Client.Factories;
using WestCoastEducation.Client.ViewModels;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Services;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Repositories;


namespace WestCoastEducation.Client.Helpers;

public static class ServiceCollectionExtensions
{
    // Syftet är att skapa en metod som lägger till tjänser services
    // i ServiceCollection objektet. Som vi fått via Microsoft.Extensions.DependencyInjection...
    public static void AddServices(this IServiceCollection collection)
    {
        // Om någon efterfrågar interface IProductService så ger
        // vi dem en instans av KalleAnka objektet...
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<PageViewFactory>();
        collection.AddTransient<HomeViewModel>();
        collection.AddTransient<StaffViewModel>();
        collection.AddSingleton<IStorageProvider, DataBaseClient>();
        

        collection.AddSingleton<StaffService>();
        collection.AddSingleton<StudentService>();
        collection.AddSingleton<CourseService>();
        collection.AddSingleton<School>();

    }
}
