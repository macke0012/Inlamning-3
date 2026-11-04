using System;
using Microsoft.Extensions.DependencyInjection;
using WestCoastEducation.Client.Factories;
using WestCoastEducation.Client.ViewModels;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Services;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Repositories;
using WestCoastEducation.Client.Enums;
using WestCoastEducation.Client.ViewModels.Courses;
using WestCoastEducation.Client.ViewModels.Students;
using WestCoastEducation.Client.ViewModels.Teachers;


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
        collection.AddTransient<CourseViewModel>();
        collection.AddTransient<TeacherViewModel>();
        collection.AddTransient<StudentViewModel>();
        collection.AddTransient<AddTeacherViewModel>();
        collection.AddSingleton<IStorageProvider, DataBaseClient>();
        

        collection.AddSingleton<StaffService>();
        collection.AddSingleton<StudentService>();
        collection.AddSingleton<CourseService>();
        collection.AddSingleton<School>();

        collection.AddSingleton<Func<PageViewsEnum, ViewModelBase>>(c => name => name switch
        {
            PageViewsEnum.HOME => c.GetRequiredService<HomeViewModel>(),
            PageViewsEnum.COURSES => c.GetRequiredService<CourseViewModel>(),
            PageViewsEnum.TEACHER => c.GetRequiredService<TeacherViewModel>(),
            PageViewsEnum.STUDENT => c.GetRequiredService<StudentViewModel>(),
            PageViewsEnum.ADD_TEACHER => c.GetRequiredService<AddTeacherViewModel>(),
            _ => throw new InvalidOperationException()
        });

    }
}
